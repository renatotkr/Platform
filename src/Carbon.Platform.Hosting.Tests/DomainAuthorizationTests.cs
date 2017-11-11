using System;

using Xunit;

namespace Carbon.Platform.Hosting.Tests
{
    public class DomainAuthorizationTests
    {
        [Fact]
		public void A()
		{
            var now = DateTime.UtcNow;

            var authorization = new DomainAuthorization(
                id          : 1,
                type        : DomainAuthorizationType.Acme,
                properties  : null,
                completed   : now,
                expires     : now.AddYears(1)
            );

            Assert.Equal(DomainAuthorizationType.Acme, authorization.Type);
            Assert.Equal(now, authorization.Completed);
            Assert.False(authorization.IsExpired);
            Assert.True(authorization.IsValid);
        }
        
    }
}