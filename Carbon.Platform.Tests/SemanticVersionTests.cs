namespace Carbon.Platform.Tests
{
	using Xunit;
	
	public class SemanticVersionTests
	{
        [Fact]
        public void RangeTests()
        {
            var v = Semver.Parse("1.x.x");
            var r = v.GetRange();

            Assert.Equal(VersionCategory.Major, v.Level);

            Assert.Equal(new Semver(1, 0, 0), r.Start);
            Assert.Equal(new Semver(1, 999, 9999), r.End);

            Assert.Equal("001.999.9999", r.End.ToAlignedString());

            Assert.True(r.Contains(new Semver(1, 0, 0)));
            Assert.True(r.Contains(new Semver(1, 3, 9)));

            Assert.False(r.Contains(new Semver(0, 1, 0)));
            Assert.False(r.Contains(new Semver(2, 0, 0)));


            v = Semver.Parse("1.0.0");
            r = v.GetRange();

            Assert.Equal(VersionCategory.Patch, v.Level);
       
            Assert.Equal(new Semver(1, 0, 0), r.Start);
            Assert.Equal(new Semver(1, 0, 0), r.End);

            Assert.True(r.Contains(new Semver(1, 0, 0)));

            Assert.False(r.Contains(new Semver(1, 0, 1)));


        }

        [Fact]
		public void FormattingTests()
		{
            Assert.Equal("1.0.0",   new Semver(1, 0, 0).ToString());
            Assert.Equal("1.2.0",   new Semver(1, 2, 0).ToString());
            Assert.Equal("1.2.3",   new Semver(1, 2, 3).ToString());
            Assert.Equal("1.x.x",   new Semver(1, -1, -1).ToString());
            Assert.Equal("latest",  new Semver(-1, -1, -1).ToString());
        }

        [Fact]
        public void FormattingTestsFormatted()
        {
            Assert.Equal("001.000.0000", new Semver(1, 0, 0).ToAlignedString());
            Assert.Equal("001.002.0000", new Semver(1, 2, 0).ToAlignedString());
            Assert.Equal("001.002.0003", new Semver(1, 2, 3).ToAlignedString());
            Assert.Equal("100.234.3019", new Semver(100, 234, 3019).ToAlignedString());
        }

        [Fact]
        public void EqualityTests()
        {
            Assert.Equal(new Semver(1, 0, 0), new Semver(1, 0, 0));
            Assert.Equal(new Semver(1, 1, 1), new Semver(1, 1, 1));

            // Not Equal
            Assert.NotEqual(new Semver(1, 1, 1), new Semver(1, 2, 1));
        }

        [Fact]
        public void ComparsionTests()
        {
            Assert.True(new Semver(2) > new Semver(1));
            Assert.True(new Semver(1, 1, 1) > new Semver(1, 1, 0));

            Assert.True(new Semver(1) < new Semver(2));
            Assert.True(new Semver(1, 1, 0) < new Semver(1, 1, 1));
        }

        [Fact]
        public void ParseTests()
        {
            Assert.Equal("1.0.0", Semver.Parse("001.000.0000").ToString());
            Assert.Equal("1.2.0", Semver.Parse("001.002.0000").ToString());
            Assert.Equal("1.2.3", Semver.Parse("001.002.0003").ToString());
            Assert.Equal("100.234.3019", Semver.Parse("100.234.3019").ToString());
        }
    }
}
