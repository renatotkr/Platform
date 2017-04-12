using Carbon.Platform.Resources;

namespace Carbon.Platform.Networking
{
    public interface INetworkSecurityGroup : IManagedResource
    {
        string Name { get; }

        // Rules
    }
}

// AKA NSG

/*
AWS    : sg-c27500d7    | Security Group
Azure  : ?              | Network Security Group
Google : 
*/
