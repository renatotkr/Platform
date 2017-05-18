/*
using Carbon.Data.Annotations;
using Carbon.VersionControl;

namespace Carbon.Platform.Storage
{
    [Dataset("RepositoryObjects")]
    public class RepositoryObject
    {
        [Member("id"), Key]
        public long Id { get; }
        
        // This is too long
        [Member("name"), Key]
        public string Name { get; set; }
        
        // Shortcut to the SHA1 checksum
        [Member("sha1", TypeName = "binary(20)"), Unique]
        public byte[] Sha1 { get; set; }
        
        [Member("sha3", TypeName = "binary(32)", Unique]
        public byte[] Sha3 { get; set; }

        [Member("mode")]
        public string Mode { get; set; }
        
        [Member("type")] // tree | blob
        public ObjectType Type { get; set; }

        public long RepositoryId => ScopedId.Get(Id);
    }
}

// These are unique across all repositories...
*/