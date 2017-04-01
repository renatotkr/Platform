using Carbon.Data.Annotations;

namespace Carbon.Platform.Repositories
{
    /*
    [Dataset("RepositoryObjects")]
    public class RepositoryObject
    {
        [Member("repositoryId"), Key]
        public long RepositoryId { get; set; }

        // e.g. 2.1.1
        // e.g. master
        
        [Member("revisionId"), Key]
        public long RevisionId { get; set; }
        
        [Member("path"), Key] // full path
        public string Name { get; set; }
        
        // Shortcut to the SHA1 checksum
        [Member("sha1", TypeName = "binary(20)")]
        public byte[] Sha1 { get; set; }

        // TODO: byte[] Sha3 { get; set; }
        [Member("type")]
        public ObjectType Type { get; set; }

        [Member("blobId")]
        public long BlobId { get; set; }
    }
    */

    // 1/master/templates/hi.tpl
}