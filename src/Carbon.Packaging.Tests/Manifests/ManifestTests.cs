using Carbon.Protection;
using System;
using Xunit;

namespace Carbon.Packaging.Tests
{
    public class ManifestTests
    {

        [Fact]
        public void A()
        {
            var a = new Manifest(new[] {
                new ManifestEntry("base.mtpl", GetHash("bas"), new DateTime(2015, 01, 01)),
                new ManifestEntry("templates/a.tpl", GetHash("a"), new DateTime(2015, 01, 01))
            });


            var b = new Manifest(new[] {
                new ManifestEntry("templates/a.tpl", GetHash("a"), new DateTime(2015, 01, 01)),
                new ManifestEntry("templates/b.tpl", GetHash("b"), new DateTime(2015, 01, 01)),
                new ManifestEntry("templates/c.tpl", GetHash("c"), new DateTime(2015, 01, 01))

            });

            var diff = ManifestDiff.Create(a, b);

            Assert.Equal(2, diff.Added.Count);
            Assert.Equal(1, diff.Removed.Count);
            Assert.Equal(0, diff.Modified.Count);


            Assert.Equal("base.mtpl", diff.Removed[0].Path);

            Assert.Equal("templates/b.tpl", diff.Added[0].Path);
            Assert.Equal("templates/c.tpl", diff.Added[1].Path);

        }

        [Fact]
        public void B()
        {
            var a = new Manifest(new[] {
                new ManifestEntry("templates/a.tpl", GetHash("a"), new DateTime(2015, 01, 01))
            });


            var b = new Manifest(new[] {
                new ManifestEntry("templates/a.tpl", GetHash("a1"), new DateTime(2015, 01, 01)),

            });

            var diff = ManifestDiff.Create(a, b);

            Assert.Equal(0, diff.Added.Count);
            Assert.Equal(0, diff.Removed.Count);
            Assert.Equal(1, diff.Modified.Count);


            Assert.Equal("templates/a.tpl", diff.Modified[0].Path);

        }

        public Hash GetHash(string text)
        {
            return Hash.Compute(HashType.SHA256, text);
        }
    }
}
