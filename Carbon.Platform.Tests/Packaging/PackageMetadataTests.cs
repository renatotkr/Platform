namespace Carbon.Platform.Tests
{
    using System.Linq;

	using Xunit;
	
	public class PackageMetadataTests
	{
		[Fact]
		public void Test1()
		{
			var x = @"{
  ""name"": ""module-name"",
  ""version"": ""10.3.1"",
  ""description"": ""An example module to illustrate the usage of a package.json"",
  ""author"": ""Your Name <you.name@example.org>"",
  ""contributors"": [{
  ""name"": ""Foo Bar"",
  ""email"": ""foo.bar@example.com""
}],
  ""bin"": {
  ""module-name"": ""./bin/module-name""
},
  ""scripts"": {
    ""test"": ""vows --spec --isolate"",
    ""start"": ""node index.js"",
    ""predeploy"": ""echo im about to deploy"",
    ""postdeploy"": ""echo ive deployed"",
    ""prepublish"": ""coffee --bare --compile --output lib/foo src/foo/*.coffee""
  },
  ""main"": ""lib/foo.js"",
  ""repository"": {
  ""type"": ""git"",
  ""url"": ""https://github.com/nodejitsu/browsenpm.org""
},
  ""bugs"": {
  ""url"": ""https://github.com/nodejitsu/browsenpm.org/issues""
},
  ""keywords"": [
  ""nodejitsu"",
  ""example"",
  ""browsenpm""
],
  ""dependencies"": {
    ""primus"": ""*"",
    ""async"": ""~0.8.0"",
    ""express"": ""4.2.x"",
    ""winston"": ""git://github.com/flatiron/winston#master"",
    ""bigpipe"": ""bigpipe/pagelet"",
    ""plates"": ""https://github.com/flatiron/plates/tarball/master""
  },
  ""devDependencies"": {
    ""vows"": ""^0.7.0"",
    ""assume"": ""<1.0.0 || >=2.3.1 <2.4.5 || >=2.5.2 <3.0.0"",
    ""pre-commit"": ""*""
  },
  ""preferGlobal"": true,
  ""private"": true,
  ""publishConfig"": {
  ""registry"": ""https://your-private-hosted-npm.registry.nodejitsu.com""
},
  ""subdomain"": ""foobar"",
  ""analyze"": true,
  ""license"": ""MIT""
}";
            var package = PackageMetadata.Parse(x);

            Assert.Equal(1, package.Contributors.Length);
            Assert.Equal("Foo Bar", package.Contributors[0].Name);

            Assert.Equal("module-name", package.Name);
            Assert.Equal("10.3.1", package.Version);

            Assert.Equal(6, package.Dependencies.Count);

            Assert.Equal("primus", package.Dependencies.First().Key);
            Assert.Equal("*", package.Dependencies.First().Value);

        }
    }
}