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

                if (config.ContainsKey("websitesRoot"))
                {
                    WebsitesRoot = new DirectoryInfo(config["websitesRoot"]);
                }
            }

            var driveLetter = "C";

            // Get current drive letter?

            if (AppsRoot == null)
            {
                AppsRoot = new DirectoryInfo(driveLetter + ":/apps");
            }

            if (WebsitesRoot == null)
            {
                WebsitesRoot = new DirectoryInfo(driveLetter + ":/websites");
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

        // windows : C:\\websites
        // linux   : var/websites
        public DirectoryInfo WebsitesRoot { get; }
    }
}


/*
config.json
{ 
  "appsRoot": "Z:/apps/",
  "websitesRoot": "Z:/websites/"
}
*/ 
 