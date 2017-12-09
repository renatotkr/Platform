using System.Runtime.Serialization;

namespace Carbon.Platform.Storage
{
    [DataContract]
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
}