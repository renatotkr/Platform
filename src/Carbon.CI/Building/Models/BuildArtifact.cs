using Carbon.Data.Annotations;

namespace Carbon.CI
{
    [Dataset("BuildArtifacts", Schema = CiadDb.Name)]
    public class BuildArtifact
    {
        public BuildArtifact() { }

        public BuildArtifact(long id, string name, byte[] sha256 = null)
        {
            Validate.Id(id);
            Validate.NotNullOrEmpty(name, nameof(name));

            Id     = id;
            Name   = name;
            Sha256 = sha256;
        }

        // buildId | #
        [Member("id"), Key]
        public long Id { get; }
        
        [Member("name")]
        public string Name { get; }

        [Member("sha256"), FixedSize(32)]
        public byte[] Sha256 { get; }
    }
}