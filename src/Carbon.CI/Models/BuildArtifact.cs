using System;

using Carbon.Data.Annotations;

namespace Carbon.CI
{
    [Dataset("BuildArtifacts", Schema = "CI")]
    public class BuildArtifact
    {
        public BuildArtifact() { }

        public BuildArtifact(long id, string name)
        {
            #region Preconditions

            Validate.Id(id);

            Validate.NotNullOrEmpty(name);

            #endregion

            Id   = id;
            Name = name;
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