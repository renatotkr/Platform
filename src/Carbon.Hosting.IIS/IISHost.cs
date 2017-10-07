using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;

using Carbon.Logging;
using Carbon.Net;
using Carbon.Packaging;
using Carbon.Platform.Computing;
using Carbon.Storage;
using Carbon.Versioning;

using Microsoft.Web.Administration;

namespace Carbon.Hosting.IIS
{
    public class IISHost : IHostService, IDisposable
    {
        private readonly HostingEnvironment env;
        private readonly ServerManager manager = new ServerManager();
        private readonly ILogger log;

        private static readonly Firewall firewall = new Firewall();

        public IISHost(HostingEnvironment env, ILogger log)
        {
            this.env = env ?? throw new ArgumentNullException(nameof(env));
            this.log = log ?? throw new ArgumentNullException(nameof(log));
        }

        public IEnumerable<IProgram> Scan()
        {
            foreach (var site in manager.Sites)
            {
                if (site.Applications.Count == 0) continue;

                yield return FromSite(site);
            }
        }

        public IProgram Find(long id)
        {
            var site = FindSite(id);

            if (site == null) return null;

            return FromSite(site);
        }

        public Task CreateAsync(IProgram app)
        {
            string poolName = GetApplicationPoolName(app);

            ApplicationPool pool = manager.ApplicationPools[poolName];

            if (pool != null)
            {
                log.Info($"Application pool '{pool.Name}' already exists. Runtime: {pool.ManagedRuntimeVersion}. Skipping");

                return Task.CompletedTask;
            }

            pool = manager.ApplicationPools.Add(poolName); // Create a new pool

            LogInfo(app, $"Created pool for {app.Name}");

            pool.ManagedRuntimeVersion = "v4.0";                            // Set the Runtime Version to 4.0

            pool.Failure.RapidFailProtection = false;                       // Disable rapid fail protection (RapidFailPolicy)

            pool.Recycling.PeriodicRestart.Requests = 0;                    // Disable request recycling
            pool.Recycling.PeriodicRestart.Memory = 0;                      // Disable memory recycling
            pool.Recycling.PeriodicRestart.Time = TimeSpan.Zero;            // Disable time recycling

            pool.AutoStart = true;                                          // Automatically start
            pool.StartMode = StartMode.AlwaysRunning;                       // Configure AutoStart to AlwaysRunning
            pool.ProcessModel.IdleTimeout = TimeSpan.Zero;                  // Never timeout (or unload)

            // "IIS AppPool\<AppPoolName>"
            pool.ProcessModel.IdentityType    = ProcessModelIdentityType.ApplicationPoolIdentity;
            pool.ProcessModel.LoadUserProfile = true;                       // Ensure the user profile is loaded

            // Limit cpu under load
            pool.Cpu.Action = ProcessorAction.ThrottleUnderLoad;
            pool.Cpu.Limit = 65 * 1000; // 65%

            // Create the site ------------------------------------
            // - Set path to a placeholder root directory
            // - Setup bindings

            manager.Sites.Add(GetConfigureSite(app));

            // Commit the changes to the server
            manager.CommitChanges();

            return Task.CompletedTask;
        }

        public async Task DeployAsync(IProgram app, IPackage package)
        {
            #region Ensure the app exists

            if (Find(app.Id) == null)
            {
                await CreateAsync(app).ConfigureAwait(false);
            }

            #endregion

            var appRoot = GetAppPath(app);

            if (appRoot.Exists)
            {
                throw new Exception($"'{appRoot.FullName}' already exists");
            }

            try
            {
                await package.ExtractToDirectoryAsync(appRoot).ConfigureAwait(false);
            }
            catch
            {
                appRoot.Delete(true);

                throw;
            }

            #region Set Access Control

            var accountName = "IIS AppPool\\" + app.Name;

            log.Info($"Setting ACL for {accountName}");

            var accessControl = appRoot.GetAccessControl(); // Current settings

            accessControl.AddAccessRule(new FileSystemAccessRule(
                identity         : accountName,
                fileSystemRights : FileSystemRights.ReadData | FileSystemRights.ReadAndExecute,
                inheritanceFlags : InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
                propagationFlags : PropagationFlags.None,
                type             : AccessControlType.Allow
            ));

            try
            {
                appRoot.SetAccessControl(accessControl);
            }
            catch (Exception ex)
            {
                log.Error($"Error creating ACL. {ex.Message}");
            }

            #endregion

            // Ensure the rights have propogated
            await Task.Delay(TimeSpan.FromMilliseconds(30)).ConfigureAwait(false);

            LogInfo(app, $"Deployed v{app.Version}");
        }

        public IEnumerable<SemanticVersion> GetDeployedVersions(IProgram app)
        {
            var root = GetAppRootDirectory(app);

            foreach (var folder in new DirectoryInfo(root).EnumerateDirectories())
            {
                SemanticVersion version;

                try
                {
                    version = SemanticVersion.Parse(folder.Name);

                }
                catch
                {
                    continue;
                }

                yield return version;
            }
        }

        public bool IsDeployed(IProgram app)
        {
            return GetAppPath(app).Exists;
        }

        public Task ActivateAsync(IProgram app, SemanticVersion version)
        {
            var site = FindSite(app.Id);

            if (site == null)
            {
                throw new Exception($"Site #{app.Id} ({app.Name}) not found");
            }

            var application = site.Applications["/"]; // or 0

            if (application == null)
            {
                throw new Exception($"Site#{app.Id} ({app.Name}) does not have a configured application");
            }

            var pool = GetApplicationPool(site);

            // e.g. /var/apps/1/2.1.3
            var newPath = GetAppPath(app);

            if (!newPath.Exists)
            {
                throw new Exception($"Directory '{newPath.FullName}' does not exist. Has the application been deployed?");
            }

            // Set the path (or symbolic link) to the specified version
            application.VirtualDirectories["/"].PhysicalPath = newPath.FullName;

            manager.CommitChanges();

            pool.Recycle();  // Recycle the application pool

            log.Info($"Recycled app pool {pool.Name}");

            LogInfo(app, $"Activated {app.Name} v{version}");

            return Task.CompletedTask;
        }

        public Task RestartAsync(IProgram app)
        {
            var site = FindSite(app.Id);

            var pool = GetApplicationPool(site);

            pool.Recycle();

            return Task.CompletedTask;
        }

        public Task DeleteAsync(IProgram app)
        {
            #region Preconditions

            if (app == null)
                throw new ArgumentNullException(nameof(app));

            #endregion

            var site = FindSite(app.Id) ?? throw new ArgumentNullException($"No site with id #{app.Id} found");

            var pool = GetApplicationPool(site); 

            manager.Sites.Remove(site);         
            manager.ApplicationPools.Remove(pool);
       
            manager.CommitChanges();

            var dir = GetAppRootDirectory(app);

            Directory.Delete(dir, true);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            manager.Dispose();   
        }

        #region Helpers

        private string GetApplicationPoolName(IProgram app) => app.Name;

        private ApplicationPool GetApplicationPool(Site site)
        {
            return manager.ApplicationPools[site.Applications["/"].ApplicationPoolName];
        }

        private Site FindSite(long id)
        {
            foreach (var site in manager.Sites)
            {
                if (site.Id == id) return site;
            }

            return null;
        }

        private Site GetConfigureSite(IProgram app)
        {
            #region Preconditions

            if (app == null) throw new ArgumentNullException(nameof(app));

            if (app.Addresses == null || app.Addresses.Length == 0)
                throw new ArgumentException("Must have at least one address", nameof(app));

            #endregion

            var physicalPath = GetAppPath(app);

            var site = manager.Sites.CreateElement();

            site.Id   = app.Id;
            site.Name = app.Name.ToString(); // id?

            site.ServerAutoStart = true;                                // Start the server automatically
            site.LogFile.Enabled = false;                               // Disable site logging

            var siteApp = site.Applications.CreateElement();            // Create a site app

            siteApp.Path = "/";                                         // Site the root path
            siteApp.ApplicationPoolName = GetApplicationPoolName(app);  // Site the pool name

            site.Applications.Add(siteApp);

            // Create a virtual directory
            var virtualDirectory = siteApp.VirtualDirectories.CreateElement();

            virtualDirectory.Path = "/";
            virtualDirectory.PhysicalPath = physicalPath.ToString();

            siteApp.VirtualDirectories.Add(virtualDirectory);

            /*
            { 
              addresses: [ "http://carbon.com:8080" ],
            }
            */

            foreach (var address in app.Addresses)
            {
                var b = new IISBinding(Listener.Parse(address));

                log.Info("configuring binding:" + b.ToString());

                var binding = site.Bindings.CreateElement();

                binding.Protocol = b.Protocol;
                binding.BindingInformation = b.ToString();

                site.Bindings.Add(binding);

                var firewallRuleName = "app" + app.Id;

                if (!firewall.Exists(firewallRuleName, (ushort)b.Port))
                {
                    log.Info("- opening firewall port:" + b.Port);

                    firewall.Open(firewallRuleName, (ushort)b.Port);
                }
            }

            return site;
        }

        private void LogInfo(IProgram app, string message)
        {
            var logsDir = Path.Combine(env.AppsRoot.FullName, app.Id.ToString(), "logs");

            var path = Path.Combine(env.AppsRoot.FullName, app.Id.ToString(), "logs", "deploy.txt");

            // Make sure the directory exists
            if (!Directory.Exists(logsDir))
            {
                Directory.CreateDirectory(logsDir);
            }

            var line = DateTime.UtcNow.ToString("yyyy/MM/dd @ HH:mm:ss") + " : " + message;

            File.AppendAllLines(path, new[] { line });
        }

        private string GetAppRootDirectory(IProgram app)
        {
            return Path.Combine(env.AppsRoot.FullName, app.Id.ToString());
        }

        // linux   : /var/apps/1.0.0
        // windows : c:/apps/1/1.0.0

        private DirectoryInfo GetAppPath(IProgram app)
        {
            var path = Path.Combine(env.AppsRoot.FullName, app.Id.ToString(), app.Version.ToString());

            return new DirectoryInfo(path);
        }

        public List<Request> GetActiveRequests(IProgram app, TimeSpan elapsedFilter)
        {
            var pool = manager.ApplicationPools[app.Name];

            var requests = new List<Request>();

            foreach (var process in pool.WorkerProcesses)
            {
                foreach (var request in process.GetRequests((int)elapsedFilter.TotalMilliseconds))
                {
                    requests.Add(request);
                }
            }

            return requests;
        }

        public IISApplication FromSite(Site site)
        {
            var application = site.Applications["/"];
            var directory = application.VirtualDirectories["/"];

            SemanticVersion version;
            var versionText = directory.PhysicalPath.Split(Path.DirectorySeparatorChar).Last();

            try
            {
                version = SemanticVersion.Parse(versionText);
            }
            catch
            {
                throw new Exception($"Unexpected app version text: '{versionText}' / {directory.PhysicalPath}");
            }

            // TODO: Get the name...

            // 1/1.5.0

            string name = null;

            try
            {
                name = site.Applications[0].ApplicationPoolName;

            }
            catch(Exception ex)
            {
                log.Error("error getting name:" + ex.Message);
            }

            return new IISApplication(
                id      : site.Id,
                name    : name,
                version : version
            );
        }

        #endregion

        // Start, Stop
    }
}