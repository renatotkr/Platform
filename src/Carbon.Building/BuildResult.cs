using System;
using System.Runtime.Serialization;

namespace Carbon.Building
{
    using Diagnostics;

    public class BuildResult
    {
        public TimeSpan WaitTime { get; set; }

        [DataMember(Name = "elapsed")]
        public TimeSpan Elapsed { get; set; }

        public BuildStatus Status { get; set; }

        [DataMember(Name = "diagnostics")]
        public DiagnosticList Diagnostics { get; } = new DiagnosticList();
    }
}