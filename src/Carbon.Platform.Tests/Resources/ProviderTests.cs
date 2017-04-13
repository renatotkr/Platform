using Xunit;

namespace Carbon.Platform.Resources
{
	public class ProviderTests
	{
        [Theory]
        [InlineData("aws",     "Amazon",    1)]
        [InlineData("borg",    "Borg",      2)]
        [InlineData("google",  "Google",    3)]
        [InlineData("ibm",     "IBM",       4)]
        [InlineData("azure",   "Microsoft", 5)]
        [InlineData("msft",    "Microsoft", 5)]
        [InlineData("oracle",  "Oracle",    6)]
        public void CloudProviders(string code, string name, int id)
        {
            var provider = ResourceProvider.Parse(code);

            Assert.Equal(name, provider.Name);
            Assert.Equal(id,   provider.Id);
        }

        [Theory]
        [InlineData("github",    1000)]
        [InlineData("bitbucket", 1001)]
        public void RepositoryProviders(string code, int id)
        {
            var provider = ResourceProvider.Get(id);

            Assert.Equal(code, provider.Code);
        }
    }
}