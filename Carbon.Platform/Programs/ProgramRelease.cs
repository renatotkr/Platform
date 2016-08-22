using System;

namespace Carbon.Platform
{
    using Data;
    using Data.Annotations;

    // A specific packaged version of a program (either static, or with a list of depedencies)

    [Record(TableName = "ProgramReleases")]
    public class ProgramRelease
    {
        public ProgramRelease() { }

        public ProgramRelease(IProgram program, Semver version)
        {
            #region Preconditions

            if (program == null)
                throw new ArgumentNullException(nameof(program));

            #endregion

            ProgramId = program.Id;
            Version   = version;
        }

        [Member(1), Key]
        public long ProgramId { get; set; }

        [Member(2), Key]
        public Semver Version { get; set; }

        [Member(3, MaxLength = 40)] // [StringLength(40)]
        public string Commit { get; set; }

        [Member(4)] 
        public long RepositoryId { get; set; }

        [Member(5)]
        public CryptographicHash Hash { get; set; } // Package hash

        [Member(6), Version]
        public DateTime Created { get; set; }
    }
}