using System;
using System.Runtime.Serialization;

namespace GitHub
{
    public class GitRef
    {
        [DataMember(Name = "ref")]
        public string Ref { get; set; }

        [DataMember(Name = "url")]
        public Uri Url { get; set; }

        [DataMember(Name = "object")]
        public GitObject Object { get; set; }
    }
}

/*
{
    "ref": "refs/heads/master",
    "url": "https://api.github.com/repos/octocat/Hello-World/git/refs/heads/master",
    "object": {
      "type": "commit",
      "sha": "aa218f56b14c9653891f9e74264a383fa43fefbd",
      "url": "https://api.github.com/repos/octocat/Hello-World/git/commits/aa218f56b14c9653891f9e74264a383fa43fefbd"
    }
}
*/