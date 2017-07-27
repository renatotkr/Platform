using Xunit;

namespace Carbon.VersionControl.Tests
{
    public class RevisionTests
    {
        [Fact]
        public void A()
        {
            Assert.Equal("a",               Revision.Parse("a").Name);
            Assert.Equal("head:a",          Revision.Parse("a").ToString());
            Assert.Equal(RevisionType.Head, Revision.Parse("a").Type);

        }

        [Theory]
        [InlineData("head:a",   RevisionType.Head)]
        [InlineData("commit:a", RevisionType.Commit)]
        [InlineData("tag:a",    RevisionType.Tag)]
        // [InlineData("branch:a", RevisionType.Head)]

        public void Types(string text, RevisionType type)
        {
            Assert.Equal(type, Revision.Parse(text).Type);
            Assert.Equal(text, Revision.Parse(text).ToString());
        }
        
        [Fact]
        public void Patches()
        {
            Assert.Equal("heads/a", Revision.Parse("head:a").Path);
            Assert.Equal("a",       Revision.Parse("commit:a").Path);
            Assert.Equal("tags/a",  Revision.Parse("tag:a").Path);

        }
    }
}
