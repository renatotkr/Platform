using Carbon.Versioning;
using Xunit;

namespace Carbon.Packaging.Tests
{
    public class PackageMetadataTests
    {
        [Fact]
        public void A()
        {
            var text = @"{
  ""name"": ""lefty"",
  ""version"": ""1.0.6"",
  ""dependencies"": {
    ""basejs""     : ""2.0.x"",
    ""corejs""     : ""1.6.x"",
    ""router""     : ""1.2.x"",
    ""zoomable""   : ""1.2.x"",
    ""forms""      : ""2.1.x"",
    ""lazyjs""     : ""1.2.x"",
    ""scrollable"" : ""1.2.x"",
    ""player""     : ""3.2.x"",
    
    ""app""    : ""./scripts/app.js""
  }
}";

            var metadata = PackageMetadata.Parse(text);

            Assert.Equal("lefty", metadata.Name);
            Assert.Equal(new SemanticVersion(1, 0, 6), metadata.Version);

            Assert.Equal("basejs", metadata.Dependencies[0].Name);
            Assert.Equal("corejs", metadata.Dependencies[1].Name);
            Assert.Equal("router", metadata.Dependencies[2].Name);

            Assert.Equal(SemanticVersion.Parse("2.0.0"), metadata.Dependencies[0].VersionRange.Start);
            Assert.Equal(SemanticVersion.Parse("1.6.0"), metadata.Dependencies[1].VersionRange.Start);
            Assert.Equal(SemanticVersion.Parse("1.2.0"), metadata.Dependencies[2].VersionRange.Start);

            Assert.Equal(SemanticVersion.Parse("2.0.9999"), metadata.Dependencies[0].VersionRange.End);
            Assert.Equal(SemanticVersion.Parse("1.6.9999"), metadata.Dependencies[1].VersionRange.End);
            Assert.Equal(SemanticVersion.Parse("1.2.9999"), metadata.Dependencies[2].VersionRange.End);
        }
    }
}