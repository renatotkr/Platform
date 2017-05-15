using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    public interface IMachineType : IResource
    {
        string Name { get; }
    }
}

// aws   | x1.32xlarge | Instance Type
// gcp   | ulong       | compute#machineType
// azure |             | Hardware Profile

/* 
Azure - Virtual Machine Sizes
---------------------------------------------
A0 - A4     | Generation 1 & 2
D1 - D5     | Generation 2
G3

GCP - Machine Types
---------------------------------------------
n1-standard-1
n1-standard-2
n1-standard-4
*/
