using System;
using Carbon.Data.Sequences;
using Carbon.Platform.Computing;
using Carbon.Platform.Storage;
using Carbon.Storage;
using Carbon.Versioning;

namespace Carbon.CI
{
    public class CreateProgramReleaseRequest
    {
        public CreateProgramReleaseRequest(
            ProgramInfo program, 
            SemanticVersion version, 
            IRepositoryCommit commit,
            IPackage package,
            Uid? encryptionKeyId = null)
        {
            Program         = program ?? throw new ArgumentNullException(nameof(program));
            Version         = version;
            Commit          = commit;
            Package         = package ?? throw new ArgumentNullException(nameof(package));
            EncryptionKeyId = encryptionKeyId;
        }

        public ProgramInfo Program { get; }

        public SemanticVersion Version { get; }

        public IPackage Package { get; }

        public IRepositoryCommit Commit { get; }

        public Uid? EncryptionKeyId { get; }
    }
}