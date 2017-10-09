using System;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Xunit;

namespace Carbon.Kms.Tests
{
    public class CertificateTests
    {      
        // https://github.com/golang/go/blob/master/src/crypto/x509/testdata/test-dir.crt 

        private const string certificateData =
@"-----BEGIN CERTIFICATE-----
MIIFazCCA1OgAwIBAgIJAL8a/lsnspOqMA0GCSqGSIb3DQEBCwUAMEwxCzAJBgNV
BAYTAlVLMRMwEQYDVQQIDApUZXN0LVN0YXRlMRUwEwYDVQQKDAxHb2xhbmcgVGVz
dHMxETAPBgNVBAMMCHRlc3QtZGlyMB4XDTE3MDIwMTIzNTAyN1oXDTI3MDEzMDIz
NTAyN1owTDELMAkGA1UEBhMCVUsxEzARBgNVBAgMClRlc3QtU3RhdGUxFTATBgNV
BAoMDEdvbGFuZyBUZXN0czERMA8GA1UEAwwIdGVzdC1kaXIwggIiMA0GCSqGSIb3
DQEBAQUAA4ICDwAwggIKAoICAQDzBoi43Yn30KN13PKFHu8LA4UmgCRToTukLItM
WK2Je45grs/axg9n3YJOXC6hmsyrkOnyBcx1xVNgSrOAll7fSjtChRIX72Xrloxu
XewtWVIrijqz6oylbvEmbRT3O8uynu5rF82Pmdiy8oiSfdywjKuPnE0hjV1ZSCql
MYcXqA+f0JFD8kMv4pbtxjGH8f2DkYQz+hHXLrJH4/MEYdVMQXoz/GDzLyOkrXBN
hpMaBBqg1p0P+tRdfLXuliNzA9vbZylzpF1YZ0gvsr0S5Y6LVtv7QIRygRuLY4kF
k+UYuFq8NrV8TykS7FVnO3tf4XcYZ7r2KV5FjYSrJtNNo85BV5c3xMD3fJ2XcOWk
+oD1ATdgAM3aKmSOxNtNItKKxBe1mkqDH41NbWx7xMad78gDznyeT0tjEOltN2bM
uXU1R/jgR/vq5Ec0AhXJyL/ziIcmuV2fSl/ZxT4ARD+16tgPiIx+welTf0v27/JY
adlfkkL5XsPRrbSguISrj7JeaO/gjG3KnDVHcZvYBpDfHqRhCgrosfe26TZcTXx2
cRxOfvBjMz1zJAg+esuUzSkerreyRhzD7RpeZTwi6sxvx82MhYMbA3w1LtgdABio
9JRqZy3xqsIbNv7N46WO/qXL1UMRKb1UyHeW8g8btboz+B4zv1U0Nj+9qxPBbQui
dgL9LQIDAQABo1AwTjAdBgNVHQ4EFgQUy0/0W8nwQfz2tO6AZ2jPkEiTzvUwHwYD
VR0jBBgwFoAUy0/0W8nwQfz2tO6AZ2jPkEiTzvUwDAYDVR0TBAUwAwEB/zANBgkq
hkiG9w0BAQsFAAOCAgEAvEVnUYsIOt87rggmLPqEueynkuQ+562M8EDHSQl82zbe
xDCxeg3DvPgKb+RvaUdt1362z/szK10SoeMgx6+EQLoV9LiVqXwNqeYfixrhrdw3
ppAhYYhymdkbUQCEMHypmXP1vPhAz4o8Bs+eES1M+zO6ErBiD7SqkmBElT+GixJC
6epC9ZQFs+dw3lPlbiZSsGE85sqc3VAs0/JgpL/pb1/Eg4s0FUhZD2C2uWdSyZGc
g0/v3aXJCp4j/9VoNhI1WXz3M45nysZIL5OQgXymLqJElQa1pZ3Wa4i/nidvT4AT
Xlxc/qijM8set/nOqp7hVd5J0uG6qdwLRILUddZ6OpXd7ZNi1EXg+Bpc7ehzGsDt
3UFGzYXDjxYnK2frQfjLS8stOQIqSrGthW6x0fdkVx0y8BByvd5J6+JmZl4UZfzA
m99VxXSt4B9x6BvnY7ktzcFDOjtuLc4B/7yg9fv1eQuStA4cHGGAttsCg1X/Kx8W
PvkkeH0UWDZ9vhH9K36703z89da6MWF+bz92B0+4HoOmlVaXRkvblsNaynJnL0LC
Ayry7QBxuh5cMnDdRwJB3AVJIiJ1GVpb7aGvBOnx+s2lwRv9HWtghb+cbwwktx1M
JHyBf3GZNSWTpKY7cD8V+NnBv3UuioOVVo+XAU4LF/bYUjdRpxWADJizNtZrtFo=
-----END CERTIFICATE-----";

        [Fact]
        public void A()
        {
            var x509 = new X509Certificate2(Encoding.ASCII.GetBytes(certificateData));

            Assert.Equal("CN=test-dir, O=Golang Tests, S=Test-State, C=UK", x509.SubjectName.Name);
            Assert.Equal("CN=test-dir, O=Golang Tests, S=Test-State, C=UK", x509.Issuer);
            Assert.Equal("1/30/2027 3:50:27 PM", x509.GetExpirationDateString());

            var expires = new DateTime(2027, 1, 30, 15, 50, 27, 0, DateTimeKind.Utc);

            var certificate = new CertificateInfo(
                id       : 1,
                name     : "name",
                ownerId  : 1452,
                issuerId : 93,
                data     : x509.GetRawCertData(),
                expires  : expires
            );
            
            Assert.Equal(1,       certificate.Id);
            Assert.Equal("name",  certificate.Name);
            Assert.Equal(1452,    certificate.OwnerId);
            Assert.Equal(expires, certificate.Expires);
            Assert.Equal(93,      certificate.IssuerId);

            var x5092 = new X509Certificate2(certificate.Data);

            Assert.Equal(expires, x5092.NotAfter);

            Assert.Null(certificate.Revoked);
        }

        [Fact]
        public void RequestConstructor()
        {
            var x509 = new X509Certificate2(Encoding.ASCII.GetBytes(certificateData));

            var request = new CreateCertificateRequest(
                certificate : x509, 
                ownerId     : 1
            );

            Assert.Equal(1, request.OwnerId);
            
            Assert.Equal(new DateTime(2027, 1, 30, 15, 50, 27, 0, DateTimeKind.Utc), request.Expires);
        }
    }
}