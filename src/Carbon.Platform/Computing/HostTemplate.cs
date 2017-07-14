﻿using System;
using System.Runtime.Serialization;

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
            string name,
            IMachineType machineType,
            IImage image,
            long ownerId,
            ManagedResource resource,
            JsonObject properties = null)
        {
            #region Preconditions

            if (id <= 0)
                throw new ArgumentNullException("Must be > 0", nameof(id));

            if (machineType == null)
                throw new ArgumentNullException(nameof(machineType));

            if (image == null)
                throw new ArgumentNullException(nameof(image));

            #endregion

            Id            = id;
            OwnerId       = ownerId;
            Name          = name;
            MachineTypeId = machineType.Id;
            ImageId       = image.Id;
            ProviderId    = resource.ProviderId;
            ResourceId    = resource.ResourceId;
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

        // Program + Configuration

        [Member("startupScript")]
        [StringLength(2000)]
        public string StartupScript { get; set; }
        
        [Member("properties")]
        [StringLength(1000)]
        public JsonObject Properties { get; }

        #region IResource

        [IgnoreDataMember]
        [Member("providerId")]
        public int ProviderId { get; }

        [IgnoreDataMember]
        [Member("resourceId")]
        [Ascii, StringLength(100)]
        public string ResourceId { get; }
        
        int IManagedResource.LocationId => 0;

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
        public const string Volumes = "volumes";
        public const string Volume   = "volume";

        public const string IamRole  = "iamRole";
        public const string SubnetId = "subnetId";
        public const string KeyName  = "keyName";
    }

    public class VolumeSpec
    {
        public string DeviceName { get; set; }

        public long Size { get; set; }

        public string Type { get; set; }
    }
}