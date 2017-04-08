/*
using Carbon.Data.Annotations;

namespace Carbon.Platform.VersionControl
{
    [Dataset("RepositoryBlobs")]
    public class RepositoryBlob
    {
        public RepositoryBlob(long id, byte[] sha1, byte[] sha3)
        {
            Id = id;
            Sha1 = sha1;
            Sha3 = sha3;
        }

        [Member("id"), Key]
        public long Id { get; }
        
        [Member("sha1", TypeName = "binary(20)")]
        [Unique]
        public byte[] Sha1 { get; }

        [Member("sha3", TypeName = "binary(32)"), Optional]
        public byte[] Sha3 { get; }
    }
}
*/

// These will collide in the future
// Remove unique index and migrate to SHA3 before then.

// Git Hash Function Transaction
// https://docs.google.com/document/d/18hYAQCTsDgaFUo-VJGhT0UqyetL2LbAzkWNK1fYS8R0/edit

// Once in effect, begin calculating SHA3 checksums.

// Ability to identify an object by it's SHA-1 or SHA3-256 name