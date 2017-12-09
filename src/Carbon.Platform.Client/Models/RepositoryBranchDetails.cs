using System;
using System.Runtime.Serialization;

namespace Carbon.Platform.Storage
{
    [DataContract]
    public class RepositoryBranchDetails
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "commit", EmitDefaultValue = false)]
        public RepositoryCommitDetails Commit { get; set; }

        [DataMember(Name = "modified", EmitDefaultValue = false)]
        public DateTime Modified { get; set; }
    }
}
