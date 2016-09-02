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
    using Programming;

    public class IisAppHost : IProgramHost, IDisposable
    {
        private readonly ProgramEnvironment env;
        private readonly ServerManager serverManager = new ServerManager();

        private readonly Host host;
        private readonly ILogger log;

        public IisAppHost(Host machine, ProgramEnvironment env, ILogger log)
        {
            this.host = machine;
            this.env = env;
            this.log = log;
        }

        public IEnumerable<Process> Scan()
        {
            foreach (var site in serverManager.Sites)
            {
                if (site.Applications.Count == 0) continue;

                yield return FromSite(site);
            }
        }

        public Process Find(string slug)
        {
            var site = serverManager.Sites[slug];

            if (site == null) return null;

            return FromSite(site);
        }

        public Task CreateAsync(IProgram app)
        {
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

            var site = GetConfiguredSite(app);

            serverManager.Sites.Add(site);

            #endregion

            // Commit the changes to the server
            serverManager.CommitChanges();

            return Task.CompletedTask;
        }

        public async Task DeployAsync(IProgram app, Package package)
        {
            #region Ensure the app exists

            var instance = Find(app.Name);

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
            await Task.Delay(10).ConfigureAwait(false);

            LogInfo(app, $"Deployed v{app.Version}");
        }


        public IEnumerable<int> GetDeployedVersions(IProgram app)
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

        public bool IsDeployed(IProgram release)
            => GetAppPath(release).Exists;

        public Task<Process> ActivateAsync(IProgram app)
        {
            var site = serverManager.Sites[app.Name];

            if (site == null) throw new Exception("Site not found");

            var application = site.Applications["/"]; // or 0
            var pool = serverManager.ApplicationPools[app.Name];

            // e.g. D:/apps/portfolio/2.1.3
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

            var instance = FromSite(site);

            // Write an instance.txt file to the deploy directory
            /*/
            

            var instanceTxtFile = Path.Combine(newPath.FullName, "instance.txt");

            File.WriteAllText(instanceTxtFile, instance.GetKey());
            */

            return Task.FromResult(instance);
        }

        public Task ReloadAsync(IProgram app)
        {
            var pool = serverManager.ApplicationPools[app.Name];

            pool.Recycle();

            return Task.CompletedTask;
        }

        public Task DeleteAsync(IProgram app)
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

        private Site GetConfiguredSite(IProgram program)
        {
            #region Preconditions

            if (program == null)
                throw new ArgumentNullException(nameof(program));

            #endregion

            var physicalPath = GetAppPath(program);

            var site = serverManager.Sites.CreateElement();

            site.Id              = program.Id;
            site.Name            = program.Name;
            site.ServerAutoStart = true;                            // Start the server automatically
            site.LogFile.Enabled = false;                           // Disable site logging

            var siteApp = site.Applications.CreateElement();        // Create a site app

            siteApp.Path = "/";                                     // Site the root path
            siteApp.ApplicationPoolName = program.Name;      // Site the pool name

            site.Applications.Add(siteApp);

            // Create a virtual directory
            var virtualDirectory = siteApp.VirtualDirectories.CreateElement();

            virtualDirectory.Path = "/";
            virtualDirectory.PhysicalPath = physicalPath.ToString();

            siteApp.VirtualDirectories.Add(virtualDirectory);

            if (program is ISite)
            {
                foreach (var a in ((ISite)program).Bindings)
                {
                    var binding = site.Bindings.CreateElement();

                    binding.Protocol = a.Protocol;
                    binding.BindingInformation = a.ToString();

                    site.Bindings.Add(binding);
                }
            }

            return site;
        }

        private void LogInfo(IProgram app, string message)
        {
            var logsDir = Path.Combine(env.Root.FullName.TrimEnd('/'), app.Name, "logs");

            var path = Path.Combine(env.Root.FullName.TrimEnd('/'), app.Name, "logs", "deploy.txt");

            // Make sure the directory exists
            if (!Directory.Exists(logsDir))
            {
                Directory.CreateDirectory(logsDir);
            }

            var line = DateTime.UtcNow.ToString("yyyy/MM/dd @ HH:mm:ss") + " : " + message;

            File.AppendAllLines(path, new[] { line });
        }

        private string GetAppRoot(IProgram app)
        {
            return Path.Combine(env.Root.FullName.TrimEnd('/'), app.Name);
        }

        private DirectoryInfo GetAppPath(IProgram app)
        {
            // {root}/{name}/{version}

            var path = Path.Combine(GetAppRoot(app), app.Version.ToString());

            return new DirectoryInfo(path);
        }

        public IList<Request> GetActiveRequests(Process instance, TimeSpan elapsedFilter)
        {
            var pool = serverManager.ApplicationPools[instance.Name];

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

        public Process FromSite(Site site)
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

            var app = new Process {
                Id   = site.Id,
                Name = site.Name
            };

            return app;
        }

        #endregion

        // Start, Stop
    }
}






