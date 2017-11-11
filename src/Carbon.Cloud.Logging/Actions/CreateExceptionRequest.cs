using System;
using Carbon.Data.Sequences;
using Carbon.Json;

namespace Carbon.Platform.Diagnostics
{
    public class CreateExceptionRequest
    {
        public CreateExceptionRequest() { }

        public CreateExceptionRequest(Exception ex)
        {
            Type       = ex.GetType().ToString();
            Message    = ex.Message;
            StackTrace = ex.StackTrace;
            Created    = DateTime.UtcNow;
        }

        public long EnvironmentId { get; set; }

        public long HostId { get; set; }

        public long ProgramId { get; set; }

        public string ProgramVersion { get; set; }

        public long? IssueId { get; set; }

        public string Type { get; set; }

        public string Message { get; set; }

        public string StackTrace { get; set; }

        public Uid RequestId { get; set; }

        public JsonObject Properties { get; set; }

        public JsonObject Context { get; set; }

        public DateTime? Created { get; set; }
    }

    // TODO: Flags (Fatal) ...
}