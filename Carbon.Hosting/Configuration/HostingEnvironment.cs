using System;
using System.IO;

namespace Carbon.Hosting
{
    using Json;

    public class HostingEnvironment
    {
        private readonly DirectoryInfo root;

        public HostingEnvironment()
        {
            var configFile = new FileInfo(Path.Combine(AppContext.BaseDirectory, "config.json"));

            if (configFile != null)
            {
                var config = JsonObject.Parse(configFile);

                if (config.ContainsKey("appsRoot"))
                {
                    root = new DirectoryInfo(config["appsRoot"]);

                    return;
                }
            }
            
            root = new DirectoryInfo("C:/apps/");
        }

        public HostingEnvironment(DirectoryInfo root)
        {
            this.root = root;
        }

        // C:/apps/
        public DirectoryInfo AppsRoot => root;
    }
}


/*
config.json
{ 
  ""appsRoot"": "Z:/apps/"
}
*/ 
 