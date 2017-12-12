using Xunit;

namespace Carbon.Kms.Tests
{
    public class DistingustedNameTests
    {
        [Fact]
        public void A()
        {
            var text = "CN=test-dir, O=Golang Tests, S=Test-State, C=UK";

            var dn = DistinguishedName.Parse(text);
            
            Assert.Equal("test-dir", dn.CommonName);
            Assert.Equal("Golang Tests", dn.Organization);
           //  Assert.Equal("Test-State", dn.Region);
            Assert.Equal("UK", dn.Country);
        }
        
       [Fact]
       public void B()
       {
           var subject = DistinguishedName.Parse("C=US, ST=California, L=San Francisco, O=Wikimedia Foundation, CN=*.wikipedia.org");

           Assert.Equal("US",                   subject.Country);
           Assert.Equal("San Francisco",        subject.Locality);
           Assert.Equal("Wikimedia Foundation", subject.Organization);
           Assert.Equal("*.wikipedia.org",      subject.CommonName);
       }
      
    }
}