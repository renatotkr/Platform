using System;
using System.IO;

namespace Carbon.Hosting
{
    using Json;

    public class HostingEnvironment
    {
        public HostingEnvironment()
        {
            var configFileName = Path.Combine(AppContext.BaseDirectory, "config.json");

            if (File.Exists(configFileName))
            {
                var text = File.ReadAllText(configFileName);
                
                var config = JsonObject.Parse(text);

                if (config.ContainsKey("programsRoot"))
                {
                    ProgramsRoot = new DirectoryInfo(config["programsRoot"]);
                }

                if (config.ContainsKey("frontendsRoot"))
                {
                    ProgramsRoot = new DirectoryInfo(config["frontendsRoot"]);
                }

                if (config.ContainsKey("frontendsRoot"))
                {
                    ProgramsRoot = new DirectoryInfo(config["frontendsRoot"]);
                }
            }

            var driveLetter = "C";

            // Get current drive letter?

            if (ProgramsRoot == null)
            {
                ProgramsRoot = new DirectoryInfo(driveLetter + ":/apps/");
            }

            if (FrontendsRoot == null)
            {
                FrontendsRoot = new DirectoryInfo(driveLetter + ":/frontends/");
            }

            if (BackendsRoot == null)
            {
                BackendsRoot = new DirectoryInfo(driveLetter +  ":/backends/");
            }

            // C:/frontends/1/1.0.0/...
        }

        public HostingEnvironment(DirectoryInfo root)
        {
            ProgramsRoot = root;
        }

        // C:/programs/
        // bootstrapper, supervisor, etc 
        public DirectoryInfo ProgramsRoot { get; }

        // c:/backends/
        public DirectoryInfo BackendsRoot { get; }

        // C:/frontends/
        public DirectoryInfo FrontendsRoot { get; }
    }
}


/*
config.json
{ 
  ""programsRoot"": "Z:/programs/"
}
*/ 
 