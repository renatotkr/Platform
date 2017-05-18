using System;
using System.Runtime.Serialization;

namespace GitHub
{
    public class GitRepository
    {
        public long Id { get; set; }

        public string Name { get; set; }

        [DataMember(Name = "full_name")]
        public string FullName { get; set; }

        public GitActor Owner { get; set; }

        public bool Private { get; set; }

        [DataMember(Name ="html_url")]
        public string HtmlUrl { get; set; }

        public string Url { get; set; }

        public bool Fork { get; set; }

        [DataMember(Name = "master_branch")]
        public string MasterBranch { get; set; }

        [DataMember(Name = "created_at")]
        public DateTime CreatedAt { get; set; }

        [DataMember(Name = "pushed_at")]
        public DateTime PushedAt { get; set; }
    }
}
