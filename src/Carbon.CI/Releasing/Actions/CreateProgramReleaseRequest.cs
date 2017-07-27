using System;
using System.IO;
using Carbon.Data.Sequences;
using Carbon.Platform.Computing;
using Carbon.Storage;
using Carbon.Versioning;

namespace Carbon.CI
{
    public class CreateProgramReleaseRequest
    {
        public CreateProgramReleaseRequest(
            ProgramInfo program, 
            SemanticVersion version, 
            ProgramPackage package,
            IBuild build= null,
            IRepositoryCommit commit = null)
        {
            Program = program ?? throw new ArgumentNullException(nameof(program));
            Package = package ?? throw new ArgumentNullException(nameof(package));
            Build   = build;
            Version = version;
            Commit  = commit;
        }

        public ProgramInfo Program { get; }

        public SemanticVersion Version { get; }

        public ProgramPackage Package { get; }

        public IBuild Build { get; }

        public IRepositoryCommit Commit { get; }
    }

    public class ProgramPackage
    {
        public ProgramPackage(
            Stream stream,
            ArchiveFormat format,
            byte[] sha256,
            Uid? dekId = null)
        {
            Stream = stream ?? throw new ArgumentNullException(nameof(stream));
            Format = format;
            SHA256 = sha256 ?? throw new ArgumentNullException(nameof(sha256));
            DekId  = dekId;
        }

        public Stream Stream { get; }

        // zip || tar.gz
        public ArchiveFormat Format { get; }

        public byte[] SHA256 { get; }

        public Uid? DekId { get; }
    }
}