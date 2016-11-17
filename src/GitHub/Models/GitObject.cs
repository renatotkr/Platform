using System;
using System.Runtime.Serialization;

using Carbon.Repositories;

namespace GitHub
{
    public class GitObject
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "sha")]
        public string Sha { get; set; }

        [DataMember(Name = "url")]
        public Uri Url { get; set; }

        #region Helpers

        public ICommit ToCommit()
        {
            if (Type != "commit") throw new Exception("Not a commit. Was " + Type);

            return new GitCommit {
                Sha = Sha,
                Url = Url
            };
        }

        #endregion
    }
}

/*
{
    "type": "commit",
    "sha": "aa218f56b14c9653891f9e74264a383fa43fefbd",
	"url": "https://api.github.com/repos/octocat/Hello-World/git/commits/aa218f56b14c9653891f9e74264a383fa43fefbd"
}
*/
