using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;

using Microsoft.Web.Administration;

namespace Carbon.Platform.Hosting
{
    using Logging;

    public class IisAppHost : IAppHost, IDisposable
    {
        private readonly AppHostConfiguration config;
        private readonly ServerManager serverManager = new ServerManager();

        private readonly MachineInfo machine;
        private readonly ILogger log;

        public IisAppHost(MachineInfo machine, AppHostConfiguration config, ILogger log)
        {
            this.machine = machine;
            this.config = config;
            this.log = log;
        }

        public IEnumerable<AppInstance> Scan()
        {
            foreach (var site in serverManager.Sites)
            {
                if (site.Applications.Count == 0) continue;

                yield return FromSite(site);
            }
        }

        public AppInstance Find(string name)
        {
            var site = serverManager.Sites[name];

            if (site == null) return null;

            return FromSite(site);
        }

        public Task CreateAsync(IApp app)
        {
            int version = 0;

            if (serverManager.ApplicationPools[app.Name] != null)
            {
                log.Info($"Pool {app.Name} exists. Skipping");

                return Task.CompletedTask;
            }

            // Create a new pool
            var pool = serverManager.ApplicationPools.Add(app.Name);

            LogInfo(app, "Created pool");

            pool.ManagedRuntimeVersion = "v4.0";                    // Set the Runtime Version to 4.0

            pool.Failure.RapidFailProtection = false;               // Disable rapid fail protection (RapidFailPolicy)

            pool.Recycling.PeriodicRestart.Requests = 0;            // Disable request recycling
            pool.Recycling.PeriodicRestart.Memory = 0;              // Disable memory recycling
            pool.Recycling.PeriodicRestart.Time = TimeSpan.Zero;    // Disable time recycling

            pool.AutoStart = true;                                  // Automatically start
            pool.StartMode = StartMode.AlwaysRunning;               // Configure AutoStart to AlwaysRunning
            pool.ProcessModel.IdleTimeout = TimeSpan.Zero;          // Never timeout (or unload)

            pool.Cpu.Action = ProcessorAction.ThrottleUnderLoad;    // Limit cpu to 50% when under load
            pool.Cpu.Limit = 50 * 1000; // 50%

            #region Create a Site

            // - Set path to a placeholder root directory
            // - Setup bindings

            var site = GetConfiguredSite(app, version);

            serverManager.Sites.Add(site);

            #endregion

            // Commit the changes to the server
            serverManager.CommitChanges();

            return Task.CompletedTask;
        }

        public async Task DeployAsync(IApp app, int version, Package package)
        {
            #region Ensure the app exists

            var instance = Find(app.Name);

            if (instance == null)
            {
                await CreateAsync(app).ConfigureAwait(false);
            }

            #endregion

            var directory = GetAppPath(app, version);

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
            await Task.Delay(10).ConfigureAwait(false);

            LogInfo(app, $"Deployed v{version}");
        }


        public IEnumerable<int> GetDeployedVersions(IApp app)
        {
            var root = GetAppRoot(app);

            foreach (var folder in new DirectoryInfo(root).EnumerateDirectories())
            {
                int number;

                if (int.TryParse(folder.Name, out number))
                {
                    yield return number;
                }
            }
        }

        public bool IsDeployed(IApp app, int version)
            => GetAppPath(app, version).Exists;

        public Task<AppInstance> ActivateAsync(IApp app, int version)
        {
            var site = serverManager.Sites[app.Name];

            if (site == null) throw new Exception("Site not found");

            var application = site.Applications["/"]; // or 0
            var pool = serverManager.ApplicationPools[app.Name];

            // e.g. D:/apps/portfolio/2.1.3
            var newPath = GetAppPath(app, version);

            if (!newPath.Exists)
            {
                throw new Exception($"Directory '{newPath.FullName}' does not exist. Has the application been deployed?");
            }

            // Set the path (or symbolic link) to the specified version
            application.VirtualDirectories["/"].PhysicalPath = newPath.FullName;

            serverManager.CommitChanges();

            pool.Recycle();  // Recycle the application pool

            log.Info($"Recycled app pool {pool.Name}");

            LogInfo(app, $"Activated v{version}");

            // Write an instance.txt file to the deploy directory
            var instance = FromSite(site);

            var instanceTxtFile = Path.Combine(newPath.FullName, "instance.txt");

            File.WriteAllText(instanceTxtFile, instance.GetKey());

            return Task.FromResult(instance);
        }

        public Task ReloadAsync(IApp app)
        {
            var pool = serverManager.ApplicationPools[app.Name];

            pool.Recycle();

            return Task.CompletedTask;
        }

        public Task DeleteAsync(IApp app)
        {
            var site = serverManager.Sites[app.Name];

            serverManager.Sites.Remove(site);

            serverManager.CommitChanges();

            // Remove the AppPool & Site

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

        private Site GetConfiguredSite(IApp app, int version)
        {
            #region Preconditions

            if (app == null) throw new ArgumentNullException("app");

            #endregion

            var physicalPath = GetAppPath(app, version);

            var site = serverManager.Sites.CreateElement();

            site.Id = app.Id;
            site.Name = app.Name;
            site.ServerAutoStart = true;    // Start the server automatically
            site.LogFile.Enabled = false;   // Disable site logging

            var siteApp = site.Applications.CreateElement(); // Create a site app

            siteApp.Path = "/";                         // Site the root path
            siteApp.ApplicationPoolName = app.Name;     // Site the pool name

            site.Applications.Add(siteApp);

            // Create a virtual directory
            var virtualDirectory = siteApp.VirtualDirectories.CreateElement();

            virtualDirectory.Path = "/";
            virtualDirectory.PhysicalPath = physicalPath.ToString();

            siteApp.VirtualDirectories.Add(virtualDirectory);

            foreach (var a in ((App)app).Bindings)
            {
                var binding = site.Bindings.CreateElement();

                binding.Protocol = a.Protocol;
                binding.BindingInformation = a.ToString();

                site.Bindings.Add(binding);
            }

            return site;
        }

        private void LogInfo(IApp app, string message)
        {
            var logsDir = Path.Combine(config.Root.FullName.TrimEnd('/'), app.Name, "logs");

            var path = Path.Combine(config.Root.FullName.TrimEnd('/'), app.Name, "logs", "deploy.txt");

            // Make sure the directory exists
            if (!Directory.Exists(logsDir))
            {
                Directory.CreateDirectory(logsDir);
            }

            var line = DateTime.UtcNow.ToString("yyyy/MM/dd @ HH:mm:ss") + " : " + message;

            File.AppendAllLines(path, new[] { line });
        }

        private string GetAppRoot(IApp app)
        {
            return Path.Combine(config.Root.FullName.TrimEnd('/'), app.Name);
        }

        private DirectoryInfo GetAppPath(IApp app, float version)
        {
            // {root}/{name}/{version}

            var path = Path.Combine(GetAppRoot(app), version.ToString());

            return new DirectoryInfo(path);
        }

        public IList<Request> GetActiveRequests(AppInstance instance, TimeSpan elapsedFilter)
        {
            var pool = serverManager.ApplicationPools[instance.AppName];

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

        public AppInstance FromSite(Site site)
        {
            var application = site.Applications["/"]; // or 0
            var directory = application.VirtualDirectories["/"];

            int version;
            var versionText = directory.PhysicalPath.Split(Path.DirectorySeparatorChar).Last();

            try
            {
                version = int.Parse(versionText);
            }
            catch
            {
                throw new Exception($"Unexpected version text: '{versionText}' / {directory.PhysicalPath}");
            }

            // portfolio/1.5

            var app = new App
            {
                Name = site.Name,
                Id = (int)site.Id
            };

            return new AppInstance(app, machine)
            {
                AppVersion = version,
                Started = DateTime.UtcNow,
                Site = site
            };
        }

        #endregion

        // Start, Stop
    }
}






