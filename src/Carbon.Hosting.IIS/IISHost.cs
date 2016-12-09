using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;

// Replace with IISAdministration PowerShell Commands & move to dotnetcore

using Microsoft.Web.Administration;

namespace Carbon.Hosting.IIS
{
    using Logging;
    using Packaging;
    using Platform.Apps;
    using Platform.Networking;
    using Json;
    using Versioning;

    public class IISHost : IHostService, IDisposable
    {
        private readonly HostingEnvironment env;
        private readonly ServerManager serverManager = new ServerManager();

        private readonly ILogger log;

        public IISHost(HostingEnvironment env, ILogger log)
        {
            #region Preconditions

            if (env == null)
                throw new ArgumentNullException(nameof(env));

            if (log == null)
                throw new ArgumentNullException(nameof(log));

            #endregion

            this.env = env;
            this.log = log;
        }

        public IEnumerable<IApp> Scan()
        {
            foreach (var site in serverManager.Sites)
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

        public Task CreateAsync(IApp app)
        {
            if (serverManager.ApplicationPools[app.Name] != null)
            {
                log.Info($"Application pool '{app.Name}' already exists. Skipping");

                return Task.CompletedTask;
            }

            // Create a new pool
            var pool = serverManager.ApplicationPools.Add(app.Name);
            
            LogInfo(app, $"Created pool for {app.Name}");

            pool.ManagedRuntimeVersion = "v4.0";                    // Set the Runtime Version to 4.0

            pool.Failure.RapidFailProtection = false;               // Disable rapid fail protection (RapidFailPolicy)

            pool.Recycling.PeriodicRestart.Requests = 0;            // Disable request recycling
            pool.Recycling.PeriodicRestart.Memory = 0;              // Disable memory recycling
            pool.Recycling.PeriodicRestart.Time = TimeSpan.Zero;    // Disable time recycling

            pool.AutoStart                    = true;                       // Automatically start
            pool.StartMode                    = StartMode.AlwaysRunning;    // Configure AutoStart to AlwaysRunning
            pool.ProcessModel.IdleTimeout     = TimeSpan.Zero;              // Never timeout (or unload)

            // "IIS AppPool\<AppPoolName>"
            pool.ProcessModel.IdentityType    = ProcessModelIdentityType.ApplicationPoolIdentity;
            pool.ProcessModel.LoadUserProfile = true;                       // Ensure the user profile is loaded

            // Limit cpu under load
            pool.Cpu.Action = ProcessorAction.ThrottleUnderLoad;    
            pool.Cpu.Limit  = 65 * 1000; // 65%

            #region Create a Site

            // - Set path to a placeholder root directory
            // - Setup bindings

            var site = ConfigureSite(app);

            serverManager.Sites.Add(site);

            #endregion

            // Commit the changes to the server
            serverManager.CommitChanges();

            return Task.CompletedTask;
        }

        public async Task DeployAsync(IApp app, Package package)
        {
            #region Ensure the app exists

            var instance = Find(app.Id);

            if (instance == null)
            {
                await CreateAsync(app).ConfigureAwait(false);
            }

            #endregion

            var directory = GetAppPath(app);

            if (directory.Exists) throw new Exception($"Directory '{directory.FullName}' already exists");

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

            LogInfo(app, $"Deployed v{app.Version}");
        }

        public IEnumerable<SemanticVersion> GetDeployedVersions(IApp app)
        {
            var root = GetAppFolder(app);

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

        public bool IsDeployed(IApp release)
            => GetAppPath(release).Exists;

        public Task ActivateAsync(IApp app)
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

            var pool = serverManager.ApplicationPools[application.ApplicationPoolName];
            
            // e.g. D:/apps/1/2.1.3
            var newPath = GetAppPath(app);

            if (!newPath.Exists)
            {
                throw new Exception($"Directory '{newPath.FullName}' does not exist. Has the application been deployed?");
            }

            // Set the path (or symbolic link) to the specified version
            application.VirtualDirectories["/"].PhysicalPath = newPath.FullName;

            serverManager.CommitChanges();

            pool.Recycle();  // Recycle the application pool

            log.Info($"Recycled app pool {pool.Name}");

            LogInfo(app, $"Activated v{app.Version}");

            return Task.CompletedTask;
        }

        public Task ReloadAsync(IApp app)
        {
            var site = FindSite(app.Id);

            var poolName = site.Applications["/"].ApplicationPoolName;

            var pool = serverManager.ApplicationPools[poolName];

            pool.Recycle();

            return Task.CompletedTask;
        }

        public Task DeleteAsync(IApp app)
        {
            var site = FindSite(app.Id);

            if (site == null)
                throw new ArgumentNullException($"No site with id #{app.Id} found");

            var pool = serverManager.ApplicationPools[site.Applications["/"].ApplicationPoolName];
            
            serverManager.Sites.Remove(site);         
            serverManager.ApplicationPools.Remove(pool);
       
            serverManager.CommitChanges();

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            if (serverManager != null)
            {
                serverManager.Dispose();
            }
        }

        #region Helpers

        private Site FindSite(long id)
        {
            foreach (var site in serverManager.Sites)
            {
                if (site.Id == id) return site;
            }

            return null;
        }

        private Site ConfigureSite(IApp app)
        {
            #region Preconditions

            if (app == null)
                throw new ArgumentNullException(nameof(app));

            #endregion

            var physicalPath = GetAppPath(app);

            var site = serverManager.Sites.CreateElement();

            site.Id              = app.Id;
            site.Name            = app.Name.ToString(); // id?

            site.ServerAutoStart = true;                            // Start the server automatically
            site.LogFile.Enabled = false;                           // Disable site logging

            var siteApp = site.Applications.CreateElement();        // Create a site app

            siteApp.Path = "/";                                     // Site the root path
            siteApp.ApplicationPoolName = app.Name.ToString();      // Site the pool name

            site.Applications.Add(siteApp);

            // Create a virtual directory
            var virtualDirectory = siteApp.VirtualDirectories.CreateElement();

            virtualDirectory.Path = "/";
            virtualDirectory.PhysicalPath = physicalPath.ToString();

            siteApp.VirtualDirectories.Add(virtualDirectory);

            // env
            // { 
            //   listeners: [ "80/http", "http://carbon.com:8080" ]
            // }

            JsonObject env = null;

            if (app is App)
            {
                env = ((App)app).Env;
            }

            if (env == null)
            {

                Console.WriteLine("no env");
            }

            if (env != null && env.ContainsKey("listeners"))
            {
                foreach (var listener in (JsonArray)env["listeners"])
                {
                    var b = new WebBinding(Listener.Parse(listener));

                    var binding = site.Bindings.CreateElement();

                    binding.Protocol = b.Protocol;
                    binding.BindingInformation = b.ToString();

                    site.Bindings.Add(binding);
                }
            }
            else
            {
                Console.WriteLine("no listeners");
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

        private string GetAppFolder(IApp app)   
            => Path.Combine(env.AppsRoot.FullName, app.Id.ToString());

        // c:/apps/1/1.0.0

        private DirectoryInfo GetAppPath(IApp app)
        {
            var path = Path.Combine(env.AppsRoot.FullName, app.Id.ToString(), app.Version.ToString());

            return new DirectoryInfo(path);
        }

        public IList<Request> GetActiveRequests(IApp app, TimeSpan elapsedFilter)
        {
            var pool = serverManager.ApplicationPools[app.Name];

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

        public App FromSite(Site site)
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

            // 1/1.5.0

            return new App {
                Id      = site.Id,
                Version = version,
                Created = new DirectoryInfo(directory.PhysicalPath).CreationTimeUtc
            };
        }

        #endregion

        // Start, Stop
    }
}






