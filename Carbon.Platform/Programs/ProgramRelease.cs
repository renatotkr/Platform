using System;
using System.ComponentModel.DataAnnotations;

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

        [Key]
        public long ProgramId { get; set; }

        [Key]
        public Semver Version { get; set; }

        [StringLength(40)]
        public string Commit { get; set; }

        public long RepositoryId { get; set; }

        public CryptographicHash Hash { get; set; } // Package hash

        [Timestamp]
        public DateTime Created { get; set; }
    }
}