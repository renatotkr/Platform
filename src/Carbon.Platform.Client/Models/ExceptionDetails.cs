using System;
using System.Runtime.Serialization;
using Carbon.Data.Annotations;
using Carbon.Data.Sequences;
using Carbon.Json;

namespace Carbon.Platform.Diagnostics
{
    public class ExceptionDetails
    {
        [DataMember(Name = "id")]
        public Uid Id { get; set; }

        [DataMember(Name = "requestId", EmitDefaultValue = false)]
        public Uid? RequestId { get; set; }

        [DataMember(Name = "programId")]
        public long ProgramId { get; set; }

        [DataMember(Name = "hostId", EmitDefaultValue = false)]
        public long? HostId { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "message")]
        public string Message { get; set; }

        // [ { file, line, function } ]

        [DataMember(Name = "stackTrace"), Optional]
        public string StackTrace { get; set; }

        [DataMember(Name = "properties", EmitDefaultValue = false)]
        public JsonObject Properties { get; set; }

        [DataMember(Name = "context", EmitDefaultValue = false)]
        public JsonObject Context { get; set; }

        [DataMember(Name = "created", EmitDefaultValue = false)]
        public DateTime? Created { get; set; }
    }
}