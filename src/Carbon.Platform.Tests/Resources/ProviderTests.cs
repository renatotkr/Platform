using Xunit;

namespace Carbon.Platform.Resources
{
	public class ProviderTests
	{
        [Theory]
        [InlineData("borg",  "Borg",  1)]
        [InlineData("aws",   "AWS",   2)] // Amazon Web Services
        [InlineData("gcp",   "GCP",   3)] // Google Cloud Platform
        [InlineData("azure", "Azure", 4)] // Microsoft Azure
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

        [Theory]
        [InlineData("braintree", 2000)]
        [InlineData("paypal",    2001)]
        [InlineData("stripe",    2002)]
        public void PaymentProviders(string code, int id)
        {
            var provider = ResourceProvider.Get(id);

            Assert.Equal(code, provider.Code);
        }
    }
}