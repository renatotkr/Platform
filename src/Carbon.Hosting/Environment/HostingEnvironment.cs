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

                if (config.ContainsKey("appsRoot"))
                {
                    AppsRoot = new DirectoryInfo(config["appsRoot"]);
                }

                if (config.ContainsKey("sitesRoot"))
                {
                    SitesRoot = new DirectoryInfo(config["sitesRoot"]);
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
 