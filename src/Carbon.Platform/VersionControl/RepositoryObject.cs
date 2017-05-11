/*
using Carbon.Data.Annotations;
using Carbon.VersionControl;

namespace Carbon.Platform.VersionControl
{
    [Dataset("RepositoryObjects")]
    public class RepositoryObject
    {
        [Member("id"), Key]
        public long Id { get; }
        
        // This is too long
        [Member("path"), Key] // full path
        public string Path { get; set; }
        
        // Shortcut to the SHA1 checksum
        [Member("sha1", TypeName = "binary(20)"), Unique]
        public byte[] Sha1 { get; set; }
        
        // TODO: byte[] Sha3 { get; set; }

        [Member("mode")]
        public string Mode { get; set; }
        
        [Member("type")] // tree | blob
        public ObjectType Type { get; set; }

        public long RepositoryId => ScopedId.Get(Id);
    }
}
*/