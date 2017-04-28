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

// A host 'hosts' a primary application
// A host may be divided into containers to host mutiple applications

// Identity = EnvId + Sequence...

/*
         id                    name
google : UInt64              | compute#instance
aws    : i-07e6001e0415497e  | instance
azure  : ?
------------------------------------------------------------- 
*/