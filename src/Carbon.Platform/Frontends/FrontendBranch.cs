using System;

namespace Carbon.Platform.Frontends
{
    using Data.Annotations;

    [Dataset("FrontendBranches")]
    public class FrontendBranch
    {
        [Member("frontendId"), Key]
        public long FrontendId { get; set; }

        [Member("name"), Key]
        public string Name { get; set; }

        [Member("modified"), Timestamp]
        public DateTime Modified { get; set; }
    }
}