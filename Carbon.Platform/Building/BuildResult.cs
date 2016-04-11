using System;

namespace Carbon.Platform
{
    public class BuildResult
    {
        public TimeSpan WaitTime { get; set; }

        public TimeSpan Elapsed { get; set; }

        public BuildStatus Status { get; set; }

        public DiagnosticList Diagnostics { get; } = new DiagnosticList();
    }
}