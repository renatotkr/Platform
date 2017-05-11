using Carbon.Platform.Resources;

namespace Carbon.Platform
{
    public interface IEnvironment : IResource
    {
        EnvironmentType Type { get; }

        long AppId { get; }

        string Revision { get; } // e.g. master, 1.0.0
        
        // Variables
    }
}

// An environment spans one or more regions 
// Each enviroment region contains resources (i.e. hosts, databases, websites, encryption keys, etc)


// Enviromements service applications
// Examples include: development, alpha, beta, production

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
