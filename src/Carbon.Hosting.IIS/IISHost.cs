using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;

using Microsoft.Web.Administration;

namespace Carbon.Hosting.IIS
{
    using Logging;
    using Packaging;
    using Platform.Apps;
    using Json;
    using Storage;
    using Versioning;
    using Net;

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

        public IEnumerable<IApp> Scan()
        {
            foreach (var site in manager.Sites)
            {
                if (site.Applications.Count == 0) continue;

                yield return FromSite(site);
            }
        }

        public IApp Find(long id)
        {
            var site = FindSite(id);

            if (site == null) return null;

            return FromSite(site);
        }

        public Task CreateAsync(IApp app, SemanticVersion version, JsonObject env)
        {
            var poolName = GetApplicationPoolName(app);

            if (manager.ApplicationPools[poolName] != null)
            {
                log.Info($"Application pool '{poolName}' already exists. Skipping");

                return Task.CompletedTask;
            }

            var pool = manager.ApplicationPools.Add(poolName); // Create a new pool

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
            pool.ProcessModel.IdentityType = ProcessModelIdentityType.ApplicationPoolIdentity;
            pool.ProcessModel.LoadUserProfile = true;                       // Ensure the user profile is loaded

            // Limit cpu under load
            pool.Cpu.Action = ProcessorAction.ThrottleUnderLoad;
            pool.Cpu.Limit = 65 * 1000; // 65%

            // Create the site ------------------------------------
            // - Set path to a placeholder root directory
            // - Setup bindings

            manager.Sites.Add(GetConfigureSite(app, version, env));

            // Commit the changes to the server
            manager.CommitChanges();

            return Task.CompletedTask;
        }

        public async Task DeployAsync(IApp app, SemanticVersion version, JsonObject env, IPackage package)
        {
            #region Ensure the app exists

            var instance = Find(app.Id);

            if (instance == null)
            {
                await CreateAsync(app, version, env).ConfigureAwait(false);
            }

            #endregion

            var directory = GetAppPath(app, version);

            if (directory.Exists)
            {
                throw new Exception($"Directory '{directory.FullName}' already exists");
            }

            try
            {
                await package.ExtractToDirectoryAsync(directory).ConfigureAwait(false);
            }
            catch
            {
                directory.Delete(true);

                throw;
            }

            // TODO, download the keychain

            #region Set Access Control

            var accountName = "IIS AppPool\\" + app.Name;

            log.Info($"Setting ACL for {accountName}");

            var rights = FileSystemRights.ReadData | FileSystemRights.ReadAndExecute;

            // Current settings
            var accessControl = directory.GetAccessControl();

            // Add the FileSystemAccessRule to the security settings.
            accessControl.AddAccessRule(new FileSystemAccessRule(
                accountName,
                rights,
                InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
                PropagationFlags.None,
                AccessControlType.Allow
            ));

            try
            {
                directory.SetAccessControl(accessControl);
            }
            catch (Exception ex)
            {
                log.Error($"Error creating ACL. {ex.Message}");
            }

            #endregion

            // Ensure the rights have propogated
            await Task.Delay(TimeSpan.FromMilliseconds(30)).ConfigureAwait(false);

            LogInfo(app, $"Deployed v{version}");
        }

        public IEnumerable<SemanticVersion> GetDeployedVersions(IApp app)
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

        public bool IsDeployed(IApp release, SemanticVersion version)
        {
            return GetAppPath(release, version).Exists;
        }

        public Task ActivateAsync(IApp app, SemanticVersion version)
        {
            var site = FindSite(app.Id);

            if (site == null)
            {
                throw new Exception($"Site #{app.Id} ({app.Name}) not found");
            }

            var application = site.Applications["/"]; // or 0

            if (application == null)
            {
                throw new Exception($"Site #{app.Id} ({app.Name}) does not have a configured application");
            }

            var pool = GetApplicationPool(site);

     
            // e.g. D:/apps/1/2.1.3
            var newPath = GetAppPath(app, version);

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

        public Task RestartAsync(IApp app)
        {
            var site = FindSite(app.Id);

            var pool = GetApplicationPool(site);

            pool.Recycle();

            return Task.CompletedTask;
        }

        public Task DeleteAsync(IApp app)
        {
            var site = FindSite(app.Id);

            if (site == null)
                throw new ArgumentNullException($"No site with id #{app.Id} found");

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

        private string GetApplicationPoolName(IApp app)
        {
            return app.Name;
        }

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

        private Site GetConfigureSite(IApp app, SemanticVersion version, JsonObject env)
        {
            #region Preconditions

            if (app == null)
                throw new ArgumentNullException(nameof(app));

            #endregion

            var physicalPath = GetAppPath(app, version);

            var site = manager.Sites.CreateElement();

            site.Id              = app.Id;
            site.Name            = app.Name.ToString(); // id?

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
              listeners: [ "http://carbon.com:8080" ],
              host: "carbon.com",   
              port: 8080
            }
            */
            
           
            if (env.ContainsKey("listeners"))
            {
                foreach (var listener in (JsonArray)env["listeners"])
                {
                    var b = new IISBinding(Listener.Parse(listener));

                    log.Info("configured listener:" + b.ToString());

                    var binding = site.Bindings.CreateElement();

                    binding.Protocol = b.Protocol;
                    binding.BindingInformation = b.ToString();

                    site.Bindings.Add(binding);
                }
            }
                
            if (env.ContainsKey("port"))
            {
                var port = (int)env["port"];

                var b = new IISBinding(port: port);

                log.Info("configured port:" + b.ToString());

                var binding = site.Bindings.CreateElement();

                binding.Protocol = b.Protocol;
                binding.BindingInformation = b.ToString();

                site.Bindings.Add(binding);

                log.Info("opening port:" + port);

                firewall.Close("app" + app.Id);
                firewall.Open("app" + app.Id, (ushort)port);
            }

            if (site.Bindings.Count == 0)
            {
                log.Info("no bindings configured for app");
            }
            
            return site;
        }

        private void LogInfo(IApp app, string message)
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

        private string GetAppRootDirectory(IApp app)
        {
            return Path.Combine(env.AppsRoot.FullName, app.Id.ToString());
        }

        // c:/apps/1/1.0.0

        private DirectoryInfo GetAppPath(IApp app, SemanticVersion version)
        {
            var path = Path.Combine(env.AppsRoot.FullName, app.Id.ToString(), version.ToString());

            return new DirectoryInfo(path);
        }

        public List<Request> GetActiveRequests(IApp app, TimeSpan elapsedFilter)
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
                throw new Exception($"Unexpected version text: '{versionText}' / {directory.PhysicalPath}");
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
                version : version,
                created : new DirectoryInfo(directory.PhysicalPath).CreationTimeUtc
            );
        }

        #endregion

        // Start, Stop
    }

}