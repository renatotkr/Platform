namespace Carbon.Packaging
{
    using Data;
    using Data.Annotations;

    [Dataset("PackageFiles")]
    public class PackageFile
    {
        [Member(1), Key]
        public long PackageId { get; set; }

        [Member(2), Key]
        public Semver PackageVersion { get; set; }

        [Member(3), Key]
        public string Name { get; set; } 

        [Member(4)]
        public Hash Hash { get; set; }
    }
}