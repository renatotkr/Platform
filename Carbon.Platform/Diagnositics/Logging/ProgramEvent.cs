using System;

namespace Carbon.Platform
{
    using Data.Annotations;

    [Dataset("ProgramEvents")]
    public class ProgramEvent
    {
        [Member(1), Key]
        public long ProgramId { get; set; }

        [Member(2), Key, Timestamp]
        public long Id { get; set; }

        [Member(3)]
        public ActivityType Type { get; set; }

        [Member(4), Indexed]
        public long HostId { get; set; }

        [Member(5)]
        public TimeSpan? Duration { get; set; }
    }

    public enum ActivityType
    {
        Built       = 1,
        Packaged    = 2,
        Deployed    = 3,
        Activated   = 4,
        Reloaded    = 5
    }
}


// Pull the specified revision from the repository
// Build the source
// Package the build
// Sign the package
// Push the package to the PackageStore (S3)
// Create a record of the package (App Version)