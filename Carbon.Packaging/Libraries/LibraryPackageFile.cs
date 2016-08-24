namespace Carbon.Packaging
{
    using Data;
    using Data.Annotations;

    [Record(TableName = "LibraryPackageFiles")]
    public class LibraryPackageFile
    {
        [Member(1), Key]
        public long LibraryId { get; set; }

        [Member(2), Key]
        public Semver LibraryVersion { get; set; }

        [Member(3), Key]
        public string Name { get; set; }

        [Member(4)]
        public CryptographicHash Hash { get; set; }
    }
}