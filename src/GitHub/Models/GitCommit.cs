using System;
using System.Runtime.Serialization;

using Carbon.VersionControl;

namespace GitHub
{
    public class GitCommit : ICommit
    {
        public GitCommit() { }

        public GitCommit(string sha, string url = null)
        {
            Sha = sha ?? throw new ArgumentNullException(nameof(sha));
            Url = url;
        }

        [DataMember(Name = "sha")]
        public string Sha { get; set; }

        [DataMember(Name = "message")]
        public string Message { get; set; }

        [DataMember(Name = "url")]
        public string Url { get; set; }

        [DataMember(Name = "author")]
        public GitActor Author { get; set; }

        [DataMember(Name = "committer")]
        public GitActor Committer { get; set; }

        [DataMember(Name = "tree")]
        public GitNode Tree { get; set; }

        // Parents

        #region IGitCommit

        IActor ICommit.Author => Author;

        IActor ICommit.Committer => Committer;

        #endregion

    }

    public class GitNode
    {
        public string Url { get; set; }

        public string Sha { get; set; }
    }
}

/*
{
  "sha": "7638417db6d59f3c431d3e1f261cc637155684cd",
  "url": "https://api.github.com/repos/octocat/Hello-World/git/commits/7638417db6d59f3c431d3e1f261cc637155684cd",
  "author": {
    "date": "2014-11-07T22:01:45Z",
    "name": "Scott Chacon",
    "email": "schacon@gmail.com"
  },
  "committer": {
    "date": "2014-11-07T22:01:45Z",
    "name": "Scott Chacon",
    "email": "schacon@gmail.com"
  },
  "message": "added readme, because im a good github citizen",
  "tree": {
    "url": "https://api.github.com/repos/octocat/Hello-World/git/trees/691272480426f78a0138979dd3ce63b77f706feb",
    "sha": "691272480426f78a0138979dd3ce63b77f706feb"
  },
  "parents": [
    {
      "url": "https://api.github.com/repos/octocat/Hello-World/git/commits/1acc419d4d6a9ce985db7be48c6349a0475975b5",
      "sha": "1acc419d4d6a9ce985db7be48c6349a0475975b5"
    }
  ]
}
*/