using System;
using System.IO;

using Carbon.Json;

namespace Carbon.Hosting
{
    public class HostingEnvironment
    {
        public HostingEnvironment()
        {
            var configFileName = Path.Combine(AppContext.BaseDirectory, "config.json");

            if (File.Exists(configFileName))
            {
                var text = File.ReadAllText(configFileName);
                
                var config = JsonObject.Parse(text);

                if (config.TryGetValue("appsRoot", out var appsRootNode))
                {
                    AppsRoot = new DirectoryInfo(appsRootNode);
                }

                if (config.TryGetValue("sitesRoot", out var sitesRootNode))
                {
                    SitesRoot = new DirectoryInfo(sitesRootNode);
                }
            }

            var driveLetter = "C";

            // Get current drive letter?

            if (AppsRoot == null)
            {
                AppsRoot = new DirectoryInfo(driveLetter + ":/apps");
            }

            if (SitesRoot == null)
            {
                SitesRoot = new DirectoryInfo(driveLetter + ":/sites");
            }

            // C:/frontends/1/1.0.0/...
        }

        public HostingEnvironment(DirectoryInfo root)
        {
            AppsRoot = root;
        }

        // windows : C:\\apps 
        // linux   : var/apps
        public DirectoryInfo AppsRoot { get; }

        // windows : /mnt/c/sites
        // linux   : /var/sites
        public DirectoryInfo SitesRoot { get; }
    }
}

/*
config.json
{ 
  "appsRoot": "Z:/apps",
  "sitesRoot": "Z:/sites"
}
*/ 
 