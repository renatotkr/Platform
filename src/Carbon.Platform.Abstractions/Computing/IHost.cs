using System.Net;

namespace Carbon.Platform.Computing
{
    public interface IHost 
    {        
        long Id { get; }

        HostType Type { get; }

        IPAddress Address { get; }
    }
}

/*
      | id                 | name
gcp   | UInt64             | compute#instance
aws   | i-07e6001e0415497e | instance
azure | ?                  | Virtual Machines
------------------------------------------------------------- 
*/
