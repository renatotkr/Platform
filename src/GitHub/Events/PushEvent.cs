using System;
using System.Runtime.Serialization;

namespace GitHub.Events
{
    public class PushEvent
    {
        public string Ref { get; set; }

        public string Before { get; set; }

        public string After { get; set; }

        public bool Created { get; set; }

        public bool Deleted { get; set; }

        public bool Forced { get; set; }

        public string Compare { get; set; }

        public PushEventCommit[] Commits { get; set; }

        [DataMember(Name = "head_commit")]
        public PushEventCommit HeadCommit { get; set; }

        public GitRepository Repository { get; set; }
        
        public GitActor Pusher { get; set; }
        
        public GitUser Sender { get; set; }
    }


    public class PushEventCommit
    {
        public string Id { get; set; }

        public bool Distinct { get; set; }

        public string Message { get; set; }

        public string Url { get; set; }

        public DateTimeOffset Timestamp { get; set; }

        public GitActor Author { get; set; }

        public GitActor Committer { get; set; }

        public string[] Added { get; set; }

        public string[] Removed { get; set; }

        public string[] Modified { get; set; }
    }
}