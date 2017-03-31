using System;

namespace Carbon.Platform.Networking
{
    public interface INetworkPolicy
    {
        long Id { get; set; }

        long NetworkId { get; }

        string Name { get; set; }

        int ProviderId { get; set; }
    }
}

/*
AWS    : sg-c27500d7    | Security Group
Azure  : ?              | Network Security Group
Google : 
*/
