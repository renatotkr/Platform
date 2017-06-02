using Carbon.Platform.Resources;

namespace Carbon.Platform
{
    public interface IEnvironment : IResource
    {
        string Name { get; }

        EnvironmentType Type { get; }        
    }
}

// An environment spans one or cloud regions
// Each enviroment region contains resources (e.g. programs, hosts, databases, websites, encryption keys, etc)

// Enviromements service one or more programs (apps) from development through production

// All resources belong to an environment...

// accelerator#stable
// accelerator#beta

/*
    
/*
Environment Variables

{
  "port": 80,
  "host": "transmogrify.io",
  "framework": "net461",
  "listeners": [
    "https://*:80
   ]
}

// "http://localhost:5100", "http://localhost:5101", "http://*:5102"

{
   listener: "http://*:80",
   machineType: "t2.xlarge",
   framework: "nodejs@10.x",
   entryPoint: "funcName"
   ...
}

 */
