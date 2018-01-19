using System;

using Carbon.Data.Annotations;
using Carbon.Json;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    [Dataset("HostTemplates")]
    [UniqueIndex("ownerId", "name")]
    public class HostTemplate : IHostTemplate
    {
        public HostTemplate() { }
        
        public HostTemplate(
            long id,
            long ownerId,
            string name,
            long imageId,
            int locationId,
            long machineTypeId,
            string startupScript = null,
            JsonObject properties = null)
        {
            Ensure.IsValidId(id);
            Ensure.IsValidId(ownerId,      nameof(ownerId));
            Ensure.NotNullOrEmpty(name,     nameof(name));
            Ensure.IsValidId(imageId,       nameof(imageId));
            Ensure.IsValidId(machineTypeId, nameof(machineTypeId));
            
            Id            = id;
            OwnerId       = ownerId;
            Name          = name;
            MachineTypeId = machineTypeId;
            ImageId       = imageId;
            LocationId    = locationId; // may be 0 (global)
            StartupScript = startupScript;
            Properties    = properties ?? new JsonObject();
        }

        [Member("id"), Key(sequenceName: "hostTemplateId")]
        public long Id { get; }
        
        [Member("ownerId")]
        public long OwnerId { get; }

        [Member("name")]
        [StringLength(63)]
        public string Name { get; }

        [Member("imageId")]
        public long ImageId { get; }

        [Member("machineTypeId")]
        public long MachineTypeId { get; }
        
        [Member("startupScript")]
        [StringLength(4000)]
        public string StartupScript { get; set; }

        [Member("locationId")]
        public int LocationId { get; }

        [Member("properties")]
        [StringLength(1000)]
        public JsonObject Properties { get; }

        #region IResource

        ResourceType IResource.ResourceType => ResourceTypes.HostTemplate;

        #endregion

        #region Timestamps

        [Member("created"), Timestamp]
        public DateTime Created { get; }
        
        [Member("deleted"), Timestamp]
        public DateTime? Deleted { get; }
        
        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        #endregion
    }

    public static class HostTemplateProperties
    {
        public const string EbsOptimized     = "ebsOptimized";
        public const string SpotPrice        = "spotPrice";
        public const string KernelId         = "kernelId";
        public const string SecurityGroupIds = "securityGroupIds";
        public const string Monitoring       = "monitoring";
        
        public const string Volumes          = "volumes";  // [ { size: 100, type: "" } ]
        public const string Volume           = "volume";   // { size: 100, type: "" }
                                             
        public const string IamRole          = "iamRole";
        public const string SubnetId         = "subnetId";
        public const string KeyName          = "keyName";
    }
}