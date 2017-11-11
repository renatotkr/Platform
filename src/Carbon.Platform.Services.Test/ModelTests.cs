using System;
using Carbon.Platform.Environments;

using Xunit;

namespace Carbon.Platform.Services.Test
{
    public class ModelTests
    {
        [Fact]
        public void CreateEnvironmentRequestModel()
        {
            var request = new CreateEnvironmentRequest("a", 1);

            Assert.Equal("a", request.Name);
            Assert.Equal(1, request.OwnerId);

            Assert.Throws<ArgumentException>(() => new CreateEnvironmentRequest("",  1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new CreateEnvironmentRequest("a", -1));
        }
    }
}