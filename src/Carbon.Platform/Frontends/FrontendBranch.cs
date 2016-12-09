using System;

namespace Carbon.Platform.Frontends
{
    using Data.Annotations;

    [Dataset("FrontendBranches")]
    public class FrontendBranch
    {
        public FrontendBranch() { }

        public FrontendBranch(long frontendId, string name)
        {
            FrontendId = frontendId;
            Name = name;
        }

        public FrontendBranch(long frontendId, string name, DateTime modified)
        {
            FrontendId = frontendId;
            Name = name;
            Modified = modified;
        }

        [Member("frontendId"), Key]
        public long FrontendId { get; set; }

        [Member("name"), Key]
        public string Name { get; set; }

        [Member("modified"), Timestamp]
        public DateTime Modified { get; set; }
    }
}