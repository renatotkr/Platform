using System;

using Carbon.Repositories;

namespace Bitbucket
{
    public class BitbucketCommit : IGitCommit
    {
        public string Hash { get; set; }

        public string Message { get; set; }

        public DateTime Date { get; set; }
    }
}