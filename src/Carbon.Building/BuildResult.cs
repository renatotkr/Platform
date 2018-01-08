using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Carbon.Building
{
    using Diagnostics;

    public class BuildResult
    {
        public BuildResult(
            BuildStatus status,
            TimeSpan elapsed = default,
            List<Artifact> artifacts = null,
            DiagnosticList diagnostics = null
        )
        {
            Status = status;
            Artifacts = artifacts;
            Diagnostics = diagnostics;
            Elapsed = elapsed;
        }

        [DataMember(Name = "status")]
        public BuildStatus Status { get; }

        [DataMember(Name = "elapsed")]
        public TimeSpan Elapsed { get; }

        [DataMember(Name = "diagnostics")]
        public DiagnosticList Diagnostics { get; }

        [DataMember(Name = "artifacts", EmitDefaultValue = false)]
        public IReadOnlyList<Artifact> Artifacts { get; }
    }
}