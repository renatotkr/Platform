using System;
using Carbon.Data.Sequences;
using Carbon.Platform.Computing;
using Carbon.Storage;
using Carbon.Versioning;

namespace Carbon.CI
{
    public class PublishProgramRequest
    {
        public PublishProgramRequest() { }

        public PublishProgramRequest(
            ProgramInfo program, 
            SemanticVersion version, 
            IPackage package,
            Uid? encryptionKeyId = null)
        {
            Program         = program ?? throw new ArgumentNullException(nameof(program));
            Version         = version;
            Package         = package ?? throw new ArgumentNullException(nameof(package));
            EncryptionKeyId = encryptionKeyId;
        }

        public ProgramInfo Program { get; set; }

        public SemanticVersion Version { get; set; }

        public IPackage Package { get; set; }

        public Uid? EncryptionKeyId { get; set; }
    }
}