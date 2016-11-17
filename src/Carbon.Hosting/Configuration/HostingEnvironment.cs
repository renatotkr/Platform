using System;
using System.IO;

namespace Carbon.Hosting
{
    using Json;

    public class HostingEnvironment
    {
        public HostingEnvironment()
        {
            var configFile = new FileInfo(Path.Combine(AppContext.BaseDirectory, "config.json"));

            if (configFile != null)
            {
                var text = configFile.OpenText().ReadToEnd();

                var config = JsonObject.Parse(text);

                if (config.ContainsKey("appsRoot"))
                {
                    AppsRoot = new DirectoryInfo(config["appsRoot"]);

                    return;
                }
            }

            AppsRoot = new DirectoryInfo("C:/apps/");
        }

        public HostingEnvironment(DirectoryInfo root)
        {
            AppsRoot = root;
        }

        // C:/apps/
        public DirectoryInfo AppsRoot { get; }
    }
}


/*
config.json
{ 
  ""appsRoot"": "Z:/apps/"
}
*/ 
 