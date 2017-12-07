﻿using System;

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
            IMachineType machineType,
            IImage image,
            int locationId,
            string startupScript = null,
            JsonObject properties = null)
        {
            Validate.Id(id);
            Validate.Id(ownerId,          nameof(ownerId));
            Validate.NotNullOrEmpty(name, nameof(name));
            Validate.NotNull(machineType, nameof(machineType));
            Validate.NotNull(image,       nameof(image));

            Id            = id;
            OwnerId       = ownerId;
            Name          = name;
            MachineTypeId = machineType.Id;
            ImageId       = image.Id;
            LocationId    = locationId;
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

        // volumes [ { size: 100, type: "" } ]
        public const string Volumes  = "volumes";
        public const string Volume   = "volume";

        public const string IamRole  = "iamRole";
        public const string SubnetId = "subnetId";
        public const string KeyName  = "keyName";
    }
}