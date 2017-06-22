using System;

using Carbon.Data.Annotations;
using Carbon.Versioning;
using Carbon.CI;
using Carbon.Json;

namespace Carbon.Platform.Computing
{
    [Dataset("ProgramReleases", Schema = "Computing")]
    [UniqueIndex("programId", "version")]
    public class ProgramRelease : IApplication, IProgramRelease
    {
        public ProgramRelease() { }

        public ProgramRelease(
            long id,
            IProgram program,
            SemanticVersion version,
            long creatorId,
            long? buildId = null,
            long commitId = 0,
            JsonObject properties = null)
        {
            #region Preconditions

            if (id <= 0)
                throw new ArgumentException("Must be > 0", nameof(id));

            if (program == null)
                throw new ArgumentNullException(nameof(program));

            if (version == SemanticVersion.Zero)
                throw new ArgumentException("May not be 0.0.0", nameof(version));

            if (creatorId <= 0)
                throw new ArgumentException("Must be > 0", nameof(creatorId));

            #endregion

            Id          = id;
            ProgramId   = program.Id;
            ProgramName = program.Name;
            Version     = version;
            CommitId    = commitId;
            CreatorId   = creatorId;
            BuildId     = buildId;
            Properties  = properties;
        }

        // programId | #
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

        [Member("properties")]
        [StringLength(1000)]
        public JsonObject Properties { get; }

        [Member("programName")]
        [StringLength(63)]
        public string ProgramName { get; }

        #region IProgram

        string IProgram.Name => ProgramName;

        #endregion

        #region Detail Helpers

        string[] IApplication.Urls
        {
            get => (Properties.TryGetValue("urls", out var addresses))
                ? addresses.ToArrayOf<string>()
                : null;
        }

        #endregion

        #region IDeployable

        ReleaseType IRelease.Type => ReleaseType.Program;

        long IRelease.Id => Id;

        #endregion

        #region Timestamps

        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [Member("deleted")]
        public DateTime? Deleted { get; }

        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        #endregion
    }
}