using System;
using Carbon.Json;

namespace Carbon.Platform.Diagnostics
{
    public class CreateExceptionRequest
    {
        public long EnvironmentId { get; set; }

        public long ProgramId { get; set; }

        public string Type { get; set; }

        public string Message { get; set; }

        public long HostId { get; set; }

        public long SessionId { get; set; }

        public string StackTrace { get; set; }

        public JsonObject Details { get; set; }

        public JsonObject Context { get; set; }

        public DateTime Created { get; set; }
    }
}