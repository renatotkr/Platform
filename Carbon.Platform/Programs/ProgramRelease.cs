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

            Id      = program.Id;
            Name    = program.Name;
            Version = version;
        }

        [Member(1), Key]
        public long Id { get; set; }

        [Member(2), Key]
        public Semver Version { get; set; }

        [Member(3)]
        public string Name { get; set; }

        [Member(4)]
        public long RepositoryId { get; set; }

        [Member(5, MaxLength = 40)] // Commit or named tag
        public string Revision { get; set; }

        [Member(6)]
        public CryptographicHash Hash { get; set; } // Package hash

        [Member(7), Timestamp(false)]
        public DateTime Created { get; set; }

        // name@1.2.1
        public override string ToString() => Name + "@" + Version.ToString();
    }
}

// A specific packaged version of a program (either static, or with a list of depedencies)