using System;

namespace Carbon.Platform
{
    using Data;
    using Data.Annotations;

    [Record(TableName = "ProgramReleases")]
    public class ProgramRelease : IProgram
    {
        public ProgramRelease() { }

        public ProgramRelease(IProgram program, Semver version)
        {
            #region Preconditions

            if (program == null)
                throw new ArgumentNullException(nameof(program));

            #endregion

            ProgramId = program.Id;
            ProgramSlug = program.Slug;
            Version   = version;
        }

        [Member(1), Key]
        public long ProgramId { get; set; }

        [Member(2), Key]
        public Semver Version { get; set; }

        [Member(3)]
        public string ProgramSlug { get; set; }

        [Member(4, MaxLength = 40)] // [StringLength(40)]
        public string Commit { get; set; }

        [Member(5)] 
        public long RepositoryId { get; set; }

        [Member(6)]
        public CryptographicHash Hash { get; set; } // Package hash

        [Member(7), Version]
        public DateTime Created { get; set; }

        #region IProgram

        long IProgram.Id => ProgramId;

        string IProgram.Slug => ProgramSlug;

        #endregion

        // borg@1.2.1
        public override string ToString() => ProgramSlug + "@" + Version.ToString();
    }
}

// A specific packaged version of a program (either static, or with a list of depedencies)