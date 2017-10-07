using System.Runtime.Serialization;
using Carbon.Platform.Computing;
using Carbon.Platform.Resources;
using Carbon.Versioning;

namespace Carbon.Platform
{
    public class DeploymentDetails
    {
        public DeploymentDetails() { }

        public DeploymentDetails(IProgram program, params ManagedResource[] targets)
        {
            Program = new DeploymentProgramDetails { Id = program.Id, Version = program.Version };
            Targets = new DeploymentTargetDetails[targets.Length];

            for (var i = 0; i < targets.Length; i++)
            {
                Targets[i] = new DeploymentTargetDetails(targets[i]);
            }
        }

        [DataMember(Name = "id", EmitDefaultValue = false)]
        public long Id { get; set; }

        [DataMember(Name = "status", EmitDefaultValue = false)]
        public string Status { get; set; } // todo: DeploymentStatus

        [DataMember(Name = "program", EmitDefaultValue = false)]
        public DeploymentProgramDetails Program { get; set; }
        
        [DataMember(Name = "targets", EmitDefaultValue = false)]
        public DeploymentTargetDetails[] Targets { get; set; }
    }

    // { status: "Pending", resource: "host:1" }

    public class DeploymentProgramDetails
    {
        public DeploymentProgramDetails() { }

        public DeploymentProgramDetails(IProgram program)
        {
            Id      = program.Id;
            Version = program.Version;
        }

        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "version")]
        public SemanticVersion Version { get; set; }
    }

    public class DeploymentTargetDetails
    {
        public DeploymentTargetDetails() { }

        public DeploymentTargetDetails(ManagedResource resource)
        {
            Resource = resource;
        }

        [DataMember(Name = "status", EmitDefaultValue = false)]
        public string Status { get; set; }

        [DataMember(Name = "message", EmitDefaultValue = false)]
        public string Message { get; set; }

        // borg:host/i-12345
        // borg:cluster/i-1235123
        [DataMember(Name = "resource", EmitDefaultValue = false)]
        public ManagedResource Resource { get; set; }
    }
}