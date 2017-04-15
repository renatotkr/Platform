using System;

namespace Carbon.Platform.VersionControl
{
    public class DeleteFileRequest : IRepositoryFile
    {
        public long RepositoryId { get; set; }

        public string BranchName { get; set; }

        public string Path { get; set; }

        public long Size => 0;

        public byte[] Sha256 => null;
    }
}