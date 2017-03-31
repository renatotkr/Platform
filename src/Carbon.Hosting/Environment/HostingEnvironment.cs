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

                if (config.ContainsKey("frontendsRoot"))
                {
                    FrontendsRoot = new DirectoryInfo(config["frontendsRoot"]);
                }
            }

            var driveLetter = "C";

            // Get current drive letter?

            if (AppsRoot == null)
            {
                AppsRoot = new DirectoryInfo(driveLetter + ":/apps");
            }

            if (FrontendsRoot == null)
            {
                FrontendsRoot = new DirectoryInfo(driveLetter + ":/frontends");
            }

            // C:/frontends/1/1.0.0/...
        }

        public HostingEnvironment(DirectoryInfo root)
        {
            AppsRoot = root;
        }

        // C:/apps
        // bootstrapper, supervisor, etc 
        public DirectoryInfo AppsRoot { get; }

        // C:/frontends
        public DirectoryInfo FrontendsRoot { get; }
    }
}


/*
config.json
{ 
  ""appsRoot"": "Z:/apps/"
}
*/ 
 