using System;

using Xunit;

namespace Carbon.Platform.Tests
{
    public class ReportIdTests
    {
        [Fact]
        public void ReportIdTests1()
        {
            var reportId = ReportIdentity.Create(new DateTime(2010, 01, 01), TimeSpan.FromSeconds(30));

            Assert.Equal(5421554397609984030, reportId.Value);

            reportId = ReportIdentity.Create(new DateTime(2010, 01, 01), TimeSpan.FromSeconds(60));

            Assert.Equal(5421554397609984060, reportId.Value);
        }
    }
}