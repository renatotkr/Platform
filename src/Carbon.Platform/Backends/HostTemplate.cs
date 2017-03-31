using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Json;

namespace Carbon.Platform.Computing
{
    [Dataset("HostTemplates")]
    public class HostTemplate : IHostTemplate
    {
        [Member("id"), Key]
        public long Id { get; set; }

        [Member("machineTypeId")]
        public long MachineTypeId { get; set; }

        [Member("machineImageId")]
        public long MachineImageId { get; set; }

        // Script at startup
        [Member("script")]
        public string Script { get; set; }

        [Member("metadata")]
        [StringLength(1000)]
        public JsonObject Metadata { get; set; }

        [IgnoreDataMember]
        [Member("created"), Timestamp]
        public DateTime Created { get; }
    }
}
