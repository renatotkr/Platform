using System;
using System.Runtime.Serialization;

namespace Carbon.Platform.Storage
{
    public class RepositoryDetails // : IRepository
    {
        [DataMember(Name = "id", EmitDefaultValue = false)]
        public long Id { get; set; }

        [DataMember(Name = "name", EmitDefaultValue = false)]
        public string Name { get; set; }
        
        [DataMember(Name = "origin", EmitDefaultValue = false)]
        public string Origin { get; set; }

        [DataMember(Name = "accessToken", EmitDefaultValue = false)]
        public string AccessToken { get; set; }

        [DataMember(Name = "branches", EmitDefaultValue = false)]
        public RepositoryBranchDetails[] Branches { get; set; }

        [DataMember(Name = "commit", EmitDefaultValue = false)]
        public RepositoryCommitDetails Commit { get; set; }

        [DataMember(Name = "ownerId", EmitDefaultValue = false)]
        public long OwnerId { get; set; }
    }

    public class RepositoryBranchDetails
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "commit", EmitDefaultValue = false)]
        public RepositoryCommitDetails Commit { get; set; }

        [DataMember(Name = "modified", EmitDefaultValue = false)]
        public DateTime Modified { get; set; }
    }


    public class RepositoryCommitDetails
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "sha1", EmitDefaultValue = false)]
        public byte[] Sha1 { get; set; }

        [DataMember(Name = "message", EmitDefaultValue = false)]
        public string Message { get; set; }

        [DataMember(Name = "created", EmitDefaultValue = false)]
        public DateTime? Created { get; set; }
    }
}
