namespace Carbon.Platform.VersionControl
{
    public class CreateFileRequest : IRepositoryFile
    {
        public long RepositoryId { get; set; }

        public string BranchName { get; set; }

        public string Path { get; set; }

        public long Size { get; set; }

        public byte[] Sha256 { get; set; }

        public long CreatorId { get; set; }
    }
}