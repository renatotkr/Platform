using System;
using System.Runtime.Serialization;

namespace Carbon.Platform.ComputingUnique
{
    using Data.Annotations;
    using Json;

    [Dataset("MachineTypes")]
    [DataIndex(IndexFlags.Unique, new[] { "providerId", "name" })]
    public class MachineType : ICloudResource
    {
        [Member("id"), Key]
        public long Id { get; set; }

        [Member("providerId")]
        public int ProviderId { get; set; }

        [Member("name")]
        public string Name { get; set; }
        
        [Member("details", TypeName = "varchar(1000)")] // TODO: JSON(1000)
        public JsonObject Details { get; set; }
        
        [Member("created"), Timestamp]
        public DateTime Created { get; set; }

        #region IResource

        [IgnoreDataMember]
        public CloudProvider Provider => CloudProvider.Get(ProviderId);

        ResourceType ICloudResource.Type => ResourceType.MachineType;

        #endregion
    }
}

// id = ? ? ?

/* 
Azure (Virtual Machine Sizes
---------------------------------------------
Standard_A0
Standard_G3

Google Cloud (Machine Types)
---------------------------------------------
n1-standard-1
n1-standard-2
n1-standard-4

EC2 (Instance Types)
---------------------------------------------
c1.medium                   3 + 1 + 
c1.xlarge
c3.2xlarge
c3.4xlarge
c3.8xlarge
c3.large
c3.xlarge
c4.2xlarge
c4.4xlarge
c4.8xlarge
c4.large
c4.xlarge
cc2.8xlarge
cg1.4xlarge
cr1.8xlarge
d2.2xlarge
d2.4xlarge
d2.8xlarge
d2.xlarge
f1.16xlarge
f1.2xlarge
g2.2xlarge
g2.8xlarge
hi1.4xlarge
hs1.8xlarge
i2.2xlarge
i2.4xlarge
i2.8xlarge
i2.xlarge
m1.large
m1.medium
m1.small
m1.xlarge
m2.2xlarge
m2.4xlarge
m2.xlarge
m3.2xlarge
m3.large
m3.medium
m3.xlarge
m4.10xlarge
m4.16xlarge
m4.2xlarge
m4.4xlarge
m4.large
m4.xlarge
p2.16xlarge
p2.8xlarge
p2.xlarge
r3.2xlarge
r3.4xlarge
r3.8xlarge
r3.large
r3.xlarge
r4.16xlarge
r4.2xlarge
r4.4xlarge
r4.8xlarge
r4.large
r4.xlarge
t1.micro
t2.2xlarge
t2.large
t2.medium
t2.micro
t2.nano
t2.small
t2.xlarge
x1.16xlarge
x1.32xlarge
*/
