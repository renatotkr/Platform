using System;

using Carbon.Data.Annotations;

namespace Carbon.Platform.CI
{
    [Dataset("BuildArtifacts")]
    public class BuildArtifact
    {
        public BuildArtifact() { }

        public BuildArtifact(
            long id,
            string name, 
            byte[] sha256)
        {
            #region Preconditions

            if (id <= 0)
                throw new ArgumentException("Must be > 0", nameof(id));

            #endregion

            Id     = id;
            Name   = name ?? throw new ArgumentNullException(nameof(name));
            Sha256 = sha256;
        }

        // buildId + sequenceNumber
        [Member("id"), Key]
        public long Id { get; }
        
        [Member("name")]
        public string Name { get; }

        [Member("sha256", TypeName = "binary(32)")]
        public byte[] Sha256 { get; }
    }
}