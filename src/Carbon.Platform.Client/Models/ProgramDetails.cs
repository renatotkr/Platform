using System;
using System.Runtime.Serialization;

using Carbon.Platform.Storage;

using Carbon.Versioning;

namespace Carbon.Platform.Computing
{
    [DataContract]
    public class ProgramDetails : IProgram
    {
        [DataMember(Name = "id", EmitDefaultValue = false)]
        public long Id { get; set; }

        [DataMember(Name = "name", EmitDefaultValue = false)]
        public string Name { get; set; }
        
        [DataMember(Name = "version", EmitDefaultValue = false)]
        public SemanticVersion Version { get; set; }

        [DataMember(Name = "type", EmitDefaultValue = false)]
        public ProgramType Type { get; set; }

        [DataMember(Name = "runtime", EmitDefaultValue = false)]
        public string Runtime { get; set; }

        [DataMember(Name = "addresses", EmitDefaultValue = false)]
        public string[] Addresses { get; set; }
        
        [DataMember(Name = "repository", EmitDefaultValue = false)]
        public RepositoryDetails Repository { get; set; }

        [DataMember(Name = "commit", EmitDefaultValue = false)]
        public RepositoryCommitDetails Commit { get; set; }

        [DataMember(Name = "ownerId", EmitDefaultValue = false)]
        public long OwnerId { get; set; }

        [DataMember(Name = "parentId", EmitDefaultValue = false)]
        public long? ParentId { get; set; }

        [DataMember(Name = "slug", EmitDefaultValue = false)]
        public string Slug { get; set; }

        [DataMember(Name = "releases", EmitDefaultValue = false)]
        public InlineProgramRelease[] Releases { get; set; }
    }

    public class InlineProgramRelease
    {
        [DataMember(Name = "version")]
        public SemanticVersion Version { get; set; }

        [DataMember(Name = "created")]
        public DateTime Created { get; set; }
    }
}