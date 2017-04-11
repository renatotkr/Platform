namespace Carbon.Platform.Networking
{
    public interface INetworkSecurityGroup : IManagedResource
    {
        long Id { get; }

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
