using System;
using System.Runtime.Serialization;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Storage
{
    public class RepositoryDetails // : IRepository
    {
        [DataMember(Name = "id")]
        public long Id { get; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "origin")]
        public string Origin { get; set; }

        [DataMember(Name = "branches")]
        public RepositoryBranchDetails[] Branches { get; set; }
    }

    public class RepositoryBranchDetails
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}
