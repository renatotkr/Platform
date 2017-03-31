/*
using Xunit;

namespace Carbon.Packaging.Tests
{
    using Data;

    public class StaticRegistryTests
	{
        [Fact]
        public void Parse2()
        {
            var x = @"[ { ""source"": ""Carbon/basejs"", ""path"": ""libs/basejs/1.0.0/basejs.js"", ""name"": ""basejs"", ""version"": ""1.0.0"", ""sha256"": ""eASkQShNINKfkey6CkR8+JhugPFQC97gS5aeuKIyuNA="" }, { ""source"": ""carbon/corejs"", ""path"": ""libs/corejs/1.0.0/corejs.js"", ""name"": ""corejs"", ""version"": ""1.0.0"", ""sha256"": ""CvbWCXxwuRuZh4OJnWaUM02g565csfX+EYfIgCw97Ec="" }, { ""source"": ""carbon/router"", ""path"": ""libs/router/1.0.0/router.js"", ""name"": ""router"", ""version"": ""1.0.0"", ""sha256"": ""aQRHf4kHj2POzKnLZPbYFsRswj61UAmJ3naj2q924no="" }, { ""source"": ""carbon/zoomable"", ""path"": ""libs/zoomable/1.1.0/zoomable.js"", ""name"": ""zoomable"", ""version"": ""1.1.0"", ""sha256"": ""xfhRuVMt6ZlKuPDh+/WCpIjJtJ+7BECNJt7czgCh8Bs="" }, { ""source"": ""carbon/forms"", ""path"": ""libs/forms/1.0.0/forms.js"", ""name"": ""forms"", ""version"": ""1.0.0"", ""sha256"": ""IyqnWSAIkDHSupNuulzop2k6K4hOHJTzlj7bJSd8j9w="" }, { ""source"": ""carbon/lazyjs"", ""path"": ""libs/lazyjs/1.1.1/lazyjs.js"", ""name"": ""lazyjs"", ""version"": ""1.1.1"", ""sha256"": ""NDOQDTx1RJFD7a2wpstJvYGq8hYUA+Snsbr8Yz8WJ/4="" }, { ""source"": ""carbon/player"", ""path"": ""libs/player/2.0.0/player.js"", ""name"": ""player"", ""version"": ""2.0.0"", ""sha256"": ""9nfhfe3DfACcSD0IgOJUBW3YSeWALUK6m2S0Z8ajdXo="" } ]";

            var a = StaticPackageRegistry.Deserialize(x);

            Assert.Equal("libs/basejs/1.0.0/basejs.js", a[0].Path);
        }

		[Fact]
		public void X()
		{
            var registry = new StaticPackageRegistry();

            registry.Add(new PackageInfo("basejs", Semver.Parse("1.0.0")) { Path = "lib.js" });
            registry.Add(new PackageInfo("corejs", Semver.Parse("1.0.3")));

            var x = registry.Serialize();

            var y = StaticPackageRegistry.Deserialize(x);

            Assert.Equal("basejs", y[0].Name);
            Assert.Equal(Semver.Parse("1.0.0"), y[0].Version);
            Assert.Equal("lib.js", y[0].Path);

            Assert.Equal("corejs", y[1].Name);
            Assert.Equal(Semver.Parse("1.0.3"), y[1].Version);

            var baseLib = registry.FindAsync("basejs", Semver.Parse("1.0.0"));

            Assert.Equal("lib.js", baseLib.Path);

            var library = new PackageInfo("corejs", Semver.Parse("1.0.0"))
            {
                Path = "/libs/corejs/1.0.0/corejs.js"
            };

            var x2 = XObject.FromObject(library).ToString();

            Assert.Equal(
@"{
  ""name"": ""corejs"",
  ""version"": ""1.0.0"",
  ""path"": ""/libs/corejs/1.0.0/corejs.js""
}", x2.ToString());
        }


        [Fact]
        public void X3()
        {
            var library = new PackageInfo("corejs", Semver.Parse("1.0.0")) {
                Path = "/libs/corejs/1.0.0/corejs.js"
            };

            var x = XObject.FromObject(library).ToString();

            Assert.Equal(
@"{
  ""name"": ""corejs"",
  ""version"": ""1.0.0"",
  ""path"": ""/libs/corejs/1.0.0/corejs.js""
}", x.ToString());
        }
      
    }
}
*/