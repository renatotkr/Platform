using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    public interface IVolume : IManagedResource
    {        
        long Size { get; } // in octets
    }
}


/*              id                          | name
AWS           : vol-1234567890abcdef0       | volume
Google        : 6527490933702336850         | compute#disk
Azure         : ?                           | Managed Disk
*/

// Azure has long identifiers

// subscriptions/{subscriptionId}/resourceGroups/{resourceGroup}/providers/Microsoft.Compute/disks/{diskName}?api-version={api-version}
