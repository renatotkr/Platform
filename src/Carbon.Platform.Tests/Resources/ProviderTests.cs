using Xunit;

namespace Carbon.Platform.Resources
{
	public class ProviderTests
	{
        [Theory]
        [InlineData("borg",         "Borg",          1)]
        [InlineData("aws",          "AWS",           2)] // Amazon Web Services
        [InlineData("gcp",          "GCP",           3)] // Google Cloud Platform
        [InlineData("azure",        "Azure",         4)] // Microsoft Azure
        [InlineData("ibm",          "IBM",           5)] // IBM Cloud
        [InlineData("digitalocean", "Digital Ocean", 10)] 
        [InlineData("vultr",        "Vultr",         20)] 
        [InlineData("gcore",        "GCore",         105)]
        [InlineData("incero",       "Incero",        106)]
        [InlineData("wasabi",       "Wasabi",        107)]
        public void CloudProviders(string code, string name, int id)
        {
            var provider = ResourceProvider.Parse(code);

            Assert.Equal(id, provider.Id);
            Assert.Equal(name, provider.Name);

            // Ensure we can lookup by id too
            Assert.Equal(id, ResourceProvider.Get(id).Id);
        }

        [Theory]
        [InlineData("paypal",   5000)]
        [InlineData("braintree", 5001)]
        [InlineData("stripe", 5002)]
        public void PaymentProviders(string code, int id)
        {
            Assert.Equal(code, ResourceProvider.Get(id).Code);
            Assert.Equal(id, ResourceProvider.Parse(code).Id);
        }

        [Theory]
        [InlineData("github",    6000)]
        [InlineData("bitbucket", 6001)]
        public void RepositoryProviders(string code, int id)
        {
            Assert.Equal(code, ResourceProvider.Get(id).Code);
            Assert.Equal(id, ResourceProvider.Parse(code).Id);
        }
    }
}