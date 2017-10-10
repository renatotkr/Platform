using Xunit;

namespace Carbon.Kms.Tests
{
    public class CertificateSubjectTests
    {      
        [Fact]
        public void SubjectTest()
        {
            var request = new CertificateSubject(
                certificateId : 1,
                path          : "ai/processor"
            );

            Assert.Equal(1,               request.CertificateId);
            Assert.Equal("ai/processor",  request.Path);
        }
    }
}