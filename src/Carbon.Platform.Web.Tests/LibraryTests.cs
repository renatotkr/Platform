using System;

using Xunit;

namespace Carbon.Platform.Web.Libraries.Tests
{
    public class LibraryTests
    {
        [Fact]
        public void Test()
        {
            
            var json = @"[
  {
    ""main"": {
      ""sha256"": ""2WunAH12ucR3dwwLpTZ/fQ4P61hOK83vYwe5q96d42E="",
      ""name"": ""animation.js""
    },
    ""version"": ""1.0.2"",
    ""name"": ""animation""
  }
        ]";

            var library = StaticLibraryRegistry.Deserialize(json);

            var lib = library.Find("animation", new Versioning.SemanticVersion(1, 1, 1));

            if (lib == null) throw new Exception("lib not found");

            Assert.Equal("1.0.2", lib.Version.ToString());
            Assert.Equal("animation", lib.Name.ToString());
            Assert.Equal("animation.js", lib.Main.Name);
            Assert.Equal("2WunAH12ucR3dwwLpTZ/fQ4P61hOK83vYwe5q96d42E=", Convert.ToBase64String(lib.Main.Sha256));

            Assert.Equal(@"[
  {
    ""name"": ""animation"",
    ""version"": ""1.0.2"",
    ""main"": {
      ""name"": ""animation.js"",
      ""sha256"": ""2WunAH12ucR3dwwLpTZ/fQ4P61hOK83vYwe5q96d42E=""
    }
  }
]", library.Serialize());
        }
    }
}
