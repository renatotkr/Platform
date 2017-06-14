using System;

using Carbon.Data.Annotations;
using Carbon.Versioning;
using Carbon.CI;
using Carbon.Json;
using System.Runtime.Serialization;

namespace Carbon.Platform.Computing
{
    [Dataset("ProgramReleases", Schema = "Computing")]
    [UniqueIndex("programId", "version")]
    public class ProgramRelease : IApplication, IProgramRelease, IRelease
    {
        public ProgramRelease() { }

        public ProgramRelease(
            long id,
            IProgram program,
            SemanticVersion version,
            IPackageInfo package,
            long creatorId,
            long? buildId = null,
            long commitId = 0,
            string runtime = null,
            JsonObject properties = null)
        {
            #region Preconditions

            if (id <= 0)
                throw new ArgumentException("Must be > 0", nameof(id));

            if (program == null)
                throw new ArgumentNullException(nameof(program));

            if (version == SemanticVersion.Zero)
                throw new ArgumentException("May not be 0.0.0", nameof(version));

            if (package == null)
                throw new ArgumentNullException(nameof(package));

            if (package.Sha256.Length != 32)
                throw new ArgumentException("Must be 32 bytes", nameof(package.Sha256));

            if (creatorId <= 0)
                throw new ArgumentException("Must be > 0", nameof(creatorId));

            #endregion

            Id            = id;
            ProgramId     = program.Id;
            ProgramName   = program.Name;
            Version       = version;
            CommitId      = commitId;
            CreatorId     = creatorId;
            BuildId       = buildId;
            Runtime       = runtime;
            PackageName   = package.Name;
            PackageDekId  = package.DekId;
            PackageIV     = package.IV;
            PackageSha256 = package.Sha256;
            Properties    = properties;
        }

        // programId | #
        [Member("id"), Key]
        public long Id { get; }

        [Member("programId")]
        public long ProgramId { get; }

        [Member("programName")]
        [StringLength(63)]
        public string ProgramName { get; }

        [Member("version")]
        public SemanticVersion Version { get; }

        [Member("buildId")]
        public long? BuildId { get; }

        [Member("commitId")]
        public long CommitId { get; }

        [Member("creatorId")]
        public long CreatorId { get; }

        [Member("runtime")]
        [Ascii, StringLength(50)]
        public string Runtime { get; }

        [Member("properties")]
        [StringLength(1000)]
        public JsonObject Properties { get; }

        #region Package

        [Member("packageName")]
        [StringLength(100)]
        public string PackageName { get; }

        [Member("packageDekId")]
        public long? PackageDekId { get; }

        [Member("packageIV"), FixedSize(16)]
        public byte[] PackageIV { get; }

        [Member("packageSha256"), FixedSize(32)]
        public byte[] PackageSha256 { get; }

        [IgnoreDataMember]
        public IPackageInfo Package
        {
            get => new ProgramPackage(PackageName, PackageDekId, PackageIV, PackageSha256);
        }

        #endregion

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

        #endregion
    }

    internal class ProgramPackage : IPackageInfo
    {
        public ProgramPackage(
            string name,
            long? dekId,
            byte[] iv,
            byte[] sha256)
        {
            Name   = name;
            DekId  = dekId;
            IV     = iv;
            Sha256 = sha256;
        }

        public string Name { get; }

        public long? DekId { get; }

        public byte[] IV { get; }

        public byte[] Sha256 { get; }
    }
}