using Carbon.Data.Annotations;

namespace Carbon.Platform.Repositories
{
    [Dataset("RepositoryBlobs")]
    public class RepositoryBlob
    {
        [Member("id"), Key]
        public long Id { get; set; }
        
        // These will collide in the future
        // Remove unique index and migrate to SHA3 before then.
        [Member("sha1", TypeName = "binary(20)")]
        [Unique]
        public byte[] SHA1 { get; set; }

        [Member("sha3", TypeName = "binary(32)"), Optional]
        public byte[] SHA3 { get; set; }
    }
}

// Git Hash Function Transaction
// https://docs.google.com/document/d/18hYAQCTsDgaFUo-VJGhT0UqyetL2LbAzkWNK1fYS8R0/edit

// Once in effect, begin calculating SHA3 checksums.

// Ability to identify an object by it's SHA-1 or SHA3-256 name