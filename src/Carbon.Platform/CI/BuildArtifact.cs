using System;

using Carbon.Data.Annotations;

namespace Carbon.Platform.CI
{
    [Dataset("BuildArtifacts")]
    public class BuildArtifact
    {
        public BuildArtifact() { }

        public BuildArtifact(long id, string name)
        {
            #region Preconditions

            if (id <= 0)
                throw new ArgumentException("Must be > 0", nameof(id));

            #endregion

            Id     = id;
            Name   = name ?? throw new ArgumentNullException(nameof(name));
        }

        // buildId | #
        [Member("id"), Key]
        public long Id { get; }
        
        [Member("name")]
        public string Name { get; }

        // TODO: Choose a hash
        // Leaning toward SHA3, depending on GIT choice
    }
}