using System.Collections.Generic;

namespace Carbon.Platform
{
    using Data.Annotations;

    [Record(TableName = "Programs")]
    public class ProgramInfo : IProgram
    {
        [Identity]
        public long Id { get; set; }

        [Unique]
        public string Slug { get; set; }

        public ProgramType Type { get; set; }

        [Mutable] // highmark
        public Semver Version { get; set; }

        public long RepositoryId { get; set; }

        // TODO
        [Exclude]
        public IList<ProgramRelease> Releases { get; set; }
    }

    // TODO: Bindings
    // TODO: Permissions
}