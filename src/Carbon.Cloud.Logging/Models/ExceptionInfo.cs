using Carbon.Data.Annotations;
using Carbon.Data.Sequences;
using Carbon.Json;

namespace Carbon.Platform.Diagnostics
{
    [Dataset("Exceptions", Schema = "Logs")]
    public class ExceptionInfo : IException
    {
        // environmentId | timestamp/ms | sequenceNumber
        [Member("id"), Key]
        public Uid Id { get; set; }
     
        [Member("requestId")]
        public Uid RequestId { get; set; }

        [Member("programId")]
        public long ProgramId { get; set; }

        // [Member("programVersion")]
        [Ascii, StringLength(50)]
        public string ProgramVersion { get; set; }
        
        [Member("hostId")]
        public long? HostId { get; set; }

        [Member("type")]
        [MaxLength(200)]
        public string Type { get; set; }

        [Member("message")]
        [StringLength(1000)]
        public string Message { get; set; }
        
        // [ { file, line, function } ]

        [Member("stackTrace"), Optional]
        [StringLength(4000)]
        public string StackTrace { get; set; }
        
        // Url?

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