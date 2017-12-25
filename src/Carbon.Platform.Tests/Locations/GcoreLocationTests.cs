using Xunit;

namespace Carbon.Platform.Tests
{
    using static ResourceProvider;

    public class GcoreLocationTests
    {
        [Theory]
        [InlineData("am2", 1761607936)] // Amsterdam
        [InlineData("bl",  1761611008)] // Kazan
        [InlineData("blt", 1761612032)] // Minsk
        [InlineData("bzi", 1761617408)] // TelAviv
        [InlineData("cc1", 1761616640)] // Tokyo
        [InlineData("dc3", 1761614592)] // Ashburn
        [InlineData("k12", 1761617664)] // Saint Petersburg
        public void FromNames(string name, int id)
        {
            var location = Locations.Get(GCore, name);

            Assert.Equal(id,                  location.Id);
            Assert.Equal(name,                location.Name);
            Assert.Equal(GCore.Id,            location.ProviderId);
            Assert.Equal(LocationType.Region, location.Type);

            Assert.Equal(location, Locations.Get(GCore, name));
            Assert.Equal(location, Locations.Get(id));
        }
    }
}