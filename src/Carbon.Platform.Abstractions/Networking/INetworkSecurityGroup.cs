using Carbon.Platform.Resources;

namespace Carbon.Platform.Networking
{
    public interface INetworkSecurityGroup : IResource
    {
        string Name { get; }

        // Rules
    }
}

// aka NSG

/*
aws   | sg-c27500d7    | Security Group
azure | ?              | Network Security Group
gcp   | ? 
*/
