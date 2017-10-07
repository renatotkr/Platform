using Carbon.Json;
using Carbon.Platform;
using Carbon.Platform.Computing;
using Carbon.Platform.Resources;
using Carbon.Versioning;

using Xunit;

namespace Carbon.Deployment.Tests
{
    using static ResourceProvider;

    public class DeploymentTargetSerialization
    {
        [Fact]
        public void RoundtripSerialization()
        {
            var deployment = new DeploymentDetails {
                Status = "Failed",
                Targets = new[] {
                    new DeploymentTargetDetails(new ManagedResource(Borg, ResourceTypes.Host,        "1")),
                    new DeploymentTargetDetails(new ManagedResource(Borg, ResourceTypes.Cluster,     "2")),
                    new DeploymentTargetDetails(new ManagedResource(Borg, ResourceTypes.Environment, "3"))
                }
            };

            Assert.Equal(@"{
  ""status"": ""Failed"",
  ""targets"": [
    {
      ""resource"": ""borg:host\/1""
    },
    {
      ""resource"": ""borg:cluster\/2""
    },
    {
      ""resource"": ""borg:environment\/3""
    }
  ]
}", JsonObject.FromObject(deployment).ToString());
            
            var d2 = JsonObject.Parse(JsonObject.FromObject(deployment).ToString()).As<DeploymentDetails>();

            Assert.Equal("borg:host/1", d2.Targets[0].Resource.ToString());
        }

        [Fact]
        public void RoundtripSerialization2()
        {
            var program = new ProgramInfo(1, "carbon", "carbon", 1, new SemanticVersion(1, 1, 1));

            var deployment = new DeploymentDetails(program, new[] {
                ManagedResource.Host(Borg,        id: "1"),
                ManagedResource.Cluster(Borg,     id: "2"),
                ManagedResource.Environment(Borg, id: "3")

            });

            Assert.Equal(@"{
  ""program"": {
    ""id"": 1,
    ""version"": ""1.1.1""
  },
  ""targets"": [
    {
      ""resource"": ""borg:host\/1""
    },
    {
      ""resource"": ""borg:cluster\/2""
    },
    {
      ""resource"": ""borg:environment\/3""
    }
  ]
}", JsonObject.FromObject(deployment).ToString());


        }
    }
}