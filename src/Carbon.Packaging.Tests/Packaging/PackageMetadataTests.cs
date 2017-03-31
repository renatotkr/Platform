using System.Linq;

using Xunit;

namespace Carbon.Packaging.Tests
{
    public class PackageMetadataTests
    {
        [Fact]
        public void Velocity()
        {
            var x = @"{
  ""name"": ""velocity-animate"",
  ""version"": ""1.2.3"",
  ""description"": ""Accelerated JavaScript animation."",
  ""keywords"": [
    ""velocity"",
    ""animation"",
    ""jquery"",
    ""animate"",
    ""ui"",
    ""velocity.js"",
    ""velocityjs"",
    ""javascript""
  ],
  ""homepage"": ""http://velocityjs.org"",
  ""bugs"": {
    ""url"": ""http://github.com/julianshapiro/velocity/issues""
  },
  ""license"": ""MIT"",
  ""authors"": [
    {
      ""name"": ""Julian Shapiro"",
      ""url"": ""http://julian.com/""
    }
  ],
  ""main"": ""velocity.js"",
  ""repository"": ""julianshapiro/velocity"",
  ""files"": [
    ""velocity.js"",
    ""velocity.min.js"",
    ""velocity.ui.js"",
    ""velocity.ui.min.js""
  ],
  ""dependencies"": {
    ""jquery"": "">= 1.4.3""
  },
  ""devDependencies"": {
    ""grunt"": ""^0.4.4"",
    ""grunt-contrib-concat"": ""~0.4.0"",
    ""grunt-contrib-jshint"": ""~0.6.3"",
    ""grunt-contrib-nodeunit"": ""~0.2.0"",
    ""grunt-contrib-requirejs"": ""~0.4.4"",
    ""grunt-contrib-uglify"": ""~0.2.2""
  }
}";
            var package = PackageMetadata.Parse(x);

            Assert.Equal("velocity.js", package.Main);

            Assert.Equal(4, package.Files.Length);

            Assert.Equal("velocity.js", package.Files[0]);

        }

        [Fact]
        public void Bower()
        {
            var x = @"{
  ""name"": ""d3"",
  ""main"": ""d3.js"",
  ""scripts"": [
    ""d3.js""
  ],
  ""ignore"": [
    "".DS_Store"",
    "".git"",
    "".gitignore"",
    "".npmignore"",
    "".spmignore"",
    "".travis.yml"",
    ""Makefile"",
    ""bin"",
    ""component.json"",
    ""composer.json"",
    ""index.js"",
    ""lib"",
    ""node_modules"",
    ""package.json"",
    ""src"",
    ""test""
  ]
    }";

            var package = PackageMetadata.Parse(x);

            Assert.Equal("d3.js", package.Main);
        }

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
            Assert.Equal("10.3.1", package.Version.ToString());

            Assert.Equal(6, package.Dependencies.Count);

            Assert.Equal("primus", package.Dependencies[0].Name);
            Assert.Equal("*", package.Dependencies[0].Value);

            Assert.Equal("async", package.Dependencies[1].Name);
            Assert.Equal("~0.8.0", package.Dependencies[1].Value);

        }
    }
}