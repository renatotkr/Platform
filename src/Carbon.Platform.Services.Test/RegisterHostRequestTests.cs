using Carbon.Json;
using Carbon.Platform.Computing;

using Xunit;

namespace Carbon.Platform.Services.Test
{
    public class RegisterHostRequestTests
    {
        [Fact]
        public void A()
        {
            var json = @"{ machineType: { name: ""t2.xlarge"" } }";

            var model = JsonObject.Parse(json).As<RegisterHostRequest>();

            Assert.Null(model.MachineType.Id);
            Assert.Equal("t2.xlarge", model.MachineType.Name);
        }

        [Fact]
        public void X()
        {
            var json = JsonObject.FromObject(new {
                addresses = new[] { "192.168.1.1", "54.92.13.4" },
                resource  = "aws:us-east-1a:host/i-1234567890abcdef0",
                ownerId   = 1,
                machineType = new {
                    id = 1
                },
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

            Assert.Equal(new[] { "192.168.1.1", "54.92.13.4" },  a.Addresses);
            Assert.Equal(2,                                      a.Resource.ProviderId);
            Assert.Equal("i-1234567890abcdef0",                  a.Resource.ResourceId);
            Assert.Equal(Locations.Aws_USEast1.WithZone('A').Id, a.Resource.LocationId);
            Assert.Equal(1,                                      a.Volumes[0].Size.TotalBytes);
            Assert.Equal(1,                                      a.MachineType.Id);
        }
    }
}
