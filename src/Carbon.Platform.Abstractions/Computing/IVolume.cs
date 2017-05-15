using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    public interface IVolume : IManagedResource
    {        
        long Size { get; } // in octets
    }
}

/*     | id                    | name
aws    | vol-1234567890abcdef0 | volume
gcp    | 6527490933702336850   | compute#disk
azure  | ?                     | Managed Disk
*/

// Azure has long identifiers
// subscriptions/{subscriptionId}/resourceGroups/{resourceGroup}/providers/Microsoft.Compute/disks/{diskName}?api-version={api-version}
