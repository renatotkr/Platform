using System;

using Carbon.Data.Annotations;
using Carbon.Versioning;
using Carbon.Platform.CI;
using Carbon.Json;

namespace Carbon.Platform.Computing
{
    [Dataset("ProgramReleases", Schema = "Computing")]
    [DataIndex(IndexFlags.Unique, "programId", "version")]
    public class ProgramRelease : IProgramRelease, IRelease
    {
        public ProgramRelease() { }

        public ProgramRelease(
            long id, 
            long programId,
            SemanticVersion version,
            byte[] sha256, 
            long creatorId,
            long? buildId = null,
            long commitId = 0)
        {
            #region Preconditions

            if (programId <= 0)
                throw new ArgumentException("Must be > 0", nameof(programId));

            if (version == SemanticVersion.Zero)
                throw new ArgumentException("May not be 0.0.0", nameof(version));

            if (sha256 == null)
                throw new ArgumentNullException(nameof(sha256));

            if (sha256.Length != 32)
                throw new ArgumentException("Must be 32 bytes", nameof(sha256));

            if (creatorId <= 0)
                throw new ArgumentException("Must be > 0", nameof(creatorId));

            #endregion

            Id        = id;
            ProgramId = programId;
            Version   = version;
            CommitId  = commitId;
            Sha256    = sha256;
            CreatorId = creatorId;
            BuildId   = buildId;
        }

        // programId + sequence
        [Member("id"), Key]
        public long Id { get; }

        [Member("programId")]
        public long ProgramId { get; }
        
        [Member("version")]
        public SemanticVersion Version { get; }

        [Member("buildId")]
        public long? BuildId { get; }

        [Member("commitId")]
        public long CommitId { get; }

        [Member("creatorId")]
        public long CreatorId { get; }

        [Member("details")]
        [StringLength(1000)]
        public JsonObject Details { get; set; }

        #region IDeployable

        ReleaseType IRelease.Type => ReleaseType.Program;

        long IRelease.Id => Id;

        #endregion

        #region Hashes
        
        [Member("sha256", TypeName = "binary(32)")]
        public byte[] Sha256 { get; }

        #endregion

        #region Timestamps

        [Member("created"), Timestamp]
        public DateTime Created { get; }

        #endregion
    }

    // PackageInfo { hash: "sha-256:234234324" }
}