using Carbon.Data.Annotations;

namespace Carbon.Packaging
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