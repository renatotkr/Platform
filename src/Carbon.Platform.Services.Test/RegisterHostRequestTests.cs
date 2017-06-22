using Carbon.Json;
using Carbon.Platform.Computing;
using Carbon.Platform.Resources;

using Xunit;

namespace Carbon.Platform.Services.Test
{
    public class RegisterHostRequestTests
    {
        [Fact]
        public void X()
        {
            /*
            var request = new RegisterHostRequest {
                Addresses = new[] { "192.168.1.1" },
                Resource  = ManagedResource.Host(Locations.Aws_USEast1, "i-1")
            };

            throw new System.Exception(JsonObject.FromObject(request).ToString());
            */

            var json = JsonObject.FromObject(new {
                addresses = new[] { "192.168.1.1", "54.92.13.4" },
                resource  = "aws:us-east-1a:host/i-1234567890abcdef0",
                ownerId   = 1,
                subnet    = new {
                    resource = "aws:subnet/s-1"
                },
                volumes = new[] {
                    new {
                        size = "1"
                    }
                }
            });

            // aws/i-a

            var a = json.As<RegisterHostRequest>();

            Assert.Equal(new[] { "192.168.1.1", "54.92.13.4" }, a.Addresses);
            Assert.Equal(2, a.Resource.ProviderId);
            Assert.Equal("i-1234567890abcdef0", a.Resource.ResourceId);
            Assert.Equal(Locations.Aws_USEast1.WithZone('A').Id, a.Resource.LocationId);
            Assert.Equal(1, a.Volumes[0].Size.TotalBytes);

        }
    }
}
