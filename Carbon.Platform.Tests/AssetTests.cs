using System;
using System.IO;

using Xunit;

namespace Carbon.Packaging.Tests
{
    using Data;

    public class AssetTests
    {
        [Fact]
        public void HiddenTests()
        {
            Assert.True(new AssetStub(".asset.jpg").IsHidden());
            Assert.True(new AssetStub("/.git/asset.jpg").IsHidden());

            Assert.False(new AssetStub("asdfasdf.asdfasfd/a.asset.jpg").IsHidden());
            Assert.False(new AssetStub("/a.asset.jpg").IsHidden());
        }

        [Fact]
        public void BinarySearchAssets()
        {
            // Images
            Assert.True(new AssetStub("asset.jpg").IsStatic());
            Assert.True(new AssetStub("asset.jpeg").IsStatic());
            Assert.True(new AssetStub("asset.png").IsStatic());
            Assert.True(new AssetStub("asset.svg").IsStatic());

            Assert.True(new AssetStub("asset.swf").IsStatic());
            Assert.True(new AssetStub("asset.js").IsStatic());
            Assert.True(new AssetStub("asset.css").IsStatic());

            // HTML
            Assert.True(new AssetStub("asset.html").IsStatic());

            // Fonts
            Assert.True(new AssetStub("asset.eot").IsStatic());
            Assert.True(new AssetStub("asset.ttf").IsStatic());
            Assert.True(new AssetStub("asset.woff").IsStatic());

            Assert.False(new AssetStub("asset.mtpl").IsStatic());
            Assert.False(new AssetStub("asset.tpl").IsStatic());
        }

        public class AssetStub : IFileInfo
        {
            public AssetStub(string name)
            {
                Name = name;
            }

            public string Name { get; }

            public Stream Open()
            {
                throw new NotImplementedException();
            }

            public CryptographicHash Hash => default(CryptographicHash);

            public DateTime Modified => DateTime.UtcNow;
        }
    }
}