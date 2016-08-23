using Carbon.Data.Annotations;

namespace Carbon.Platform
{
    public class LibraryDepedency
    {
        [Member(1), Key]
        public long LibraryId { get; set; }

        [Member(2)]
        public SemverRange Version { get; set; } 

        public LibraryRelease ResolvedLibrary { get; set; }
    }
}