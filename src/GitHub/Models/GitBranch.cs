using Carbon.Repositories;
using System.Runtime.Serialization;
using System;

namespace GitHub
{
    public class GitBranch : IBranch
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "commit")]
        public GitCommit Commit { get; set; }

        #region IBranch

        ICommit IBranch.Commit => Commit;

        #endregion
    }
}

/*
{
  "name": "master",
    "commit": {
      "sha": "6dcb09b5b57875f334f61aebed695e2e4193db5e",
      "url": "https://api.github.com/repos/octocat/Hello-World/commits/c5b97d5ae6c19d5c5df71a34c7fbeeda2479ccbc"
    }
}
*/