﻿using Carbon.Data.Annotations;
using Carbon.Data.Sequences;
using Carbon.Json;

namespace Carbon.Platform.Diagnostics
{
    [Dataset("Exceptions", Schema = "Diagnostics")]
    public class ExceptionInfo : IException
    {
        // environmentId | timestamp/ms | sequenceNumber
        [Member("id"), Key]
        public Uid Id { get; set; }
     
        [Member("requestId")]
        public Uid RequestId { get; set; }

        [Member("programId")]           // Uid?
        public long ProgramId { get; set; }
        
        [Member("hostId")]
        public long? HostId { get; set; } // Uid (could contain environment...)

        [Member("type")]
        [MaxLength(1000)]
        public string Type { get; set; }

        [Member("message")]
        [StringLength(1000)]
        public string Message { get; set; }
        
        // [ { file, line, function } ]

        [Member("stackTrace"), Optional]
        [StringLength(2000)]
        public string StackTrace { get; set; }
        
        // appVersion, ...
        [Member("properties")]
        [StringLength(1000)]
        public JsonObject Properties { get; set; }
        
        [Member("context")]
        [StringLength(1000)]
        public JsonObject Context { get; set; }

        [Member("issueId"), Indexed]
        public long? IssueId { get; set; }
    }
}