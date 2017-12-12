using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Carbon.Data.Sequences;
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

        // http://fm4dd.com/openssl/certexamples.htm
        // 458 alternate names
        private const string certificateData2 =
            @"-----BEGIN CERTIFICATE-----
MIIcFzCCG4CgAwIBAgIGR09PUAFxMA0GCSqGSIb3DQEBBQUAMEYxCzAJBgNVBAYT
AlVTMRMwEQYDVQQKEwpHb29nbGUgSW5jMSIwIAYDVQQDExlHb29nbGUgSW50ZXJu
ZXQgQXV0aG9yaXR5MB4XDTEyMTAyNDEzNTczOVoXDTEzMDYwNzE5NDMyN1owZDEL
MAkGA1UEBhMCVVMxEzARBgNVBAgTCkNhbGlmb3JuaWExFjAUBgNVBAcTDU1vdW50
YWluIFZpZXcxEzARBgNVBAoTCkdvb2dsZSBJbmMxEzARBgNVBAMTCmdvb2dsZS5j
b20wgZ8wDQYJKoZIhvcNAQEBBQADgY0AMIGJAoGBAMNn/Rw5irMPscWpYsExcGQT
wqdxT/U9Pfybt9ttPYlXVbCd6yux0jWGNBHN+f4kCc5pwrbjmA4QSRY2uVa4T8f2
g3NucDDveUi29WVN+FJcyhj+V38lEkYbdhpIZL149dK5fAN1zzwCo10Nk+lhebcY
fCtMHLmuCX2D6mJ2CnPVAgMBAAGjghnwMIIZ7DAMBgNVHRMBAf8EAjAAMB0GA1Ud
JQQWMBQGCCsGAQUFBwMBBggrBgEFBQcDAjAdBgNVHQ4EFgQU0Qp1w0hi4nhbaJEB
h/wuZwO4OyIwHwYDVR0jBBgwFoAUv8Aw6/VDET5nup6R+/xq2uNrEiQwWwYDVR0f
BFQwUjBQoE6gTIZKaHR0cDovL3d3dy5nc3RhdGljLmNvbS9Hb29nbGVJbnRlcm5l
dEF1dGhvcml0eS9Hb29nbGVJbnRlcm5ldEF1dGhvcml0eS5jcmwwZgYIKwYBBQUH
AQEEWjBYMFYGCCsGAQUFBzAChkpodHRwOi8vd3d3LmdzdGF0aWMuY29tL0dvb2ds
ZUludGVybmV0QXV0aG9yaXR5L0dvb2dsZUludGVybmV0QXV0aG9yaXR5LmNydDCC
GLYGA1UdEQSCGK0wghipggpnb29nbGUuY29tggwqLmdvb2dsZS5jb22CDSoueW91
dHViZS5jb22CC3lvdXR1YmUuY29tghYqLnlvdXR1YmUtbm9jb29raWUuY29tggh5
b3V0dS5iZYILKi55dGltZy5jb22CDSouYW5kcm9pZC5jb22CC2FuZHJvaWQuY29t
ghQqLmdvb2dsZWNvbW1lcmNlLmNvbYISZ29vZ2xlY29tbWVyY2UuY29tghAqLnVy
bC5nb29nbGUuY29tggwqLnVyY2hpbi5jb22CCnVyY2hpbi5jb22CFiouZ29vZ2xl
LWFuYWx5dGljcy5jb22CFGdvb2dsZS1hbmFseXRpY3MuY29tghIqLmNsb3VkLmdv
b2dsZS5jb22CBmdvby5nbIIEZy5jb4INKi5nc3RhdGljLmNvbYIPKi5nb29nbGVh
cGlzLmNughYqLmFwcGVuZ2luZS5nb29nbGUuY29tggsqLmdvb2dsZS5hY4ILKi5n
b29nbGUuYWSCCyouZ29vZ2xlLmFlggsqLmdvb2dsZS5hZoILKi5nb29nbGUuYWeC
CyouZ29vZ2xlLmFsggsqLmdvb2dsZS5hbYILKi5nb29nbGUuYXOCCyouZ29vZ2xl
LmF0ggsqLmdvb2dsZS5heoILKi5nb29nbGUuYmGCCyouZ29vZ2xlLmJlggsqLmdv
b2dsZS5iZoILKi5nb29nbGUuYmeCCyouZ29vZ2xlLmJpggsqLmdvb2dsZS5iaoIL
Ki5nb29nbGUuYnOCCyouZ29vZ2xlLmJ5ggsqLmdvb2dsZS5jYYIMKi5nb29nbGUu
Y2F0ggsqLmdvb2dsZS5jY4ILKi5nb29nbGUuY2SCCyouZ29vZ2xlLmNmggsqLmdv
b2dsZS5jZ4ILKi5nb29nbGUuY2iCCyouZ29vZ2xlLmNpggsqLmdvb2dsZS5jbIIL
Ki5nb29nbGUuY22CCyouZ29vZ2xlLmNugg4qLmdvb2dsZS5jby5hb4IOKi5nb29n
bGUuY28uYneCDiouZ29vZ2xlLmNvLmNrgg4qLmdvb2dsZS5jby5jcoIOKi5nb29n
bGUuY28uaHWCDiouZ29vZ2xlLmNvLmlkgg4qLmdvb2dsZS5jby5pbIIOKi5nb29n
bGUuY28uaW2CDiouZ29vZ2xlLmNvLmlugg4qLmdvb2dsZS5jby5qZYIOKi5nb29n
bGUuY28uanCCDiouZ29vZ2xlLmNvLmtlgg4qLmdvb2dsZS5jby5rcoIOKi5nb29n
bGUuY28ubHOCDiouZ29vZ2xlLmNvLm1hgg4qLmdvb2dsZS5jby5teoIOKi5nb29n
bGUuY28ubnqCDiouZ29vZ2xlLmNvLnRogg4qLmdvb2dsZS5jby50eoIOKi5nb29n
bGUuY28udWeCDiouZ29vZ2xlLmNvLnVrgg4qLmdvb2dsZS5jby51eoIOKi5nb29n
bGUuY28udmWCDiouZ29vZ2xlLmNvLnZpgg4qLmdvb2dsZS5jby56YYIOKi5nb29n
bGUuY28uem2CDiouZ29vZ2xlLmNvLnp3gg8qLmdvb2dsZS5jb20uYWaCDyouZ29v
Z2xlLmNvbS5hZ4IPKi5nb29nbGUuY29tLmFpgg8qLmdvb2dsZS5jb20uYXKCDyou
Z29vZ2xlLmNvbS5hdYIPKi5nb29nbGUuY29tLmJkgg8qLmdvb2dsZS5jb20uYmiC
DyouZ29vZ2xlLmNvbS5iboIPKi5nb29nbGUuY29tLmJvgg8qLmdvb2dsZS5jb20u
YnKCDyouZ29vZ2xlLmNvbS5ieYIPKi5nb29nbGUuY29tLmJ6gg8qLmdvb2dsZS5j
b20uY26CDyouZ29vZ2xlLmNvbS5jb4IPKi5nb29nbGUuY29tLmN1gg8qLmdvb2ds
ZS5jb20uY3mCDyouZ29vZ2xlLmNvbS5kb4IPKi5nb29nbGUuY29tLmVjgg8qLmdv
b2dsZS5jb20uZWeCDyouZ29vZ2xlLmNvbS5ldIIPKi5nb29nbGUuY29tLmZqgg8q
Lmdvb2dsZS5jb20uZ2WCDyouZ29vZ2xlLmNvbS5naIIPKi5nb29nbGUuY29tLmdp
gg8qLmdvb2dsZS5jb20uZ3KCDyouZ29vZ2xlLmNvbS5ndIIPKi5nb29nbGUuY29t
Lmhrgg8qLmdvb2dsZS5jb20uaXGCDyouZ29vZ2xlLmNvbS5qbYIPKi5nb29nbGUu
Y29tLmpvgg8qLmdvb2dsZS5jb20ua2iCDyouZ29vZ2xlLmNvbS5rd4IPKi5nb29n
bGUuY29tLmxigg8qLmdvb2dsZS5jb20ubHmCDyouZ29vZ2xlLmNvbS5tdIIPKi5n
b29nbGUuY29tLm14gg8qLmdvb2dsZS5jb20ubXmCDyouZ29vZ2xlLmNvbS5uYYIP
Ki5nb29nbGUuY29tLm5mgg8qLmdvb2dsZS5jb20ubmeCDyouZ29vZ2xlLmNvbS5u
aYIPKi5nb29nbGUuY29tLm5wgg8qLmdvb2dsZS5jb20ubnKCDyouZ29vZ2xlLmNv
bS5vbYIPKi5nb29nbGUuY29tLnBhgg8qLmdvb2dsZS5jb20ucGWCDyouZ29vZ2xl
LmNvbS5waIIPKi5nb29nbGUuY29tLnBrgg8qLmdvb2dsZS5jb20ucGyCDyouZ29v
Z2xlLmNvbS5wcoIPKi5nb29nbGUuY29tLnB5gg8qLmdvb2dsZS5jb20ucWGCDyou
Z29vZ2xlLmNvbS5ydYIPKi5nb29nbGUuY29tLnNhgg8qLmdvb2dsZS5jb20uc2KC
DyouZ29vZ2xlLmNvbS5zZ4IPKi5nb29nbGUuY29tLnNsgg8qLmdvb2dsZS5jb20u
c3aCDyouZ29vZ2xlLmNvbS50aoIPKi5nb29nbGUuY29tLnRugg8qLmdvb2dsZS5j
b20udHKCDyouZ29vZ2xlLmNvbS50d4IPKi5nb29nbGUuY29tLnVhgg8qLmdvb2ds
ZS5jb20udXmCDyouZ29vZ2xlLmNvbS52Y4IPKi5nb29nbGUuY29tLnZlgg8qLmdv
b2dsZS5jb20udm6CCyouZ29vZ2xlLmN2ggsqLmdvb2dsZS5jeoILKi5nb29nbGUu
ZGWCCyouZ29vZ2xlLmRqggsqLmdvb2dsZS5ka4ILKi5nb29nbGUuZG2CCyouZ29v
Z2xlLmR6ggsqLmdvb2dsZS5lZYILKi5nb29nbGUuZXOCCyouZ29vZ2xlLmZpggsq
Lmdvb2dsZS5mbYILKi5nb29nbGUuZnKCCyouZ29vZ2xlLmdhggsqLmdvb2dsZS5n
ZYILKi5nb29nbGUuZ2eCCyouZ29vZ2xlLmdsggsqLmdvb2dsZS5nbYILKi5nb29n
bGUuZ3CCCyouZ29vZ2xlLmdyggsqLmdvb2dsZS5neYILKi5nb29nbGUuaGuCCyou
Z29vZ2xlLmhuggsqLmdvb2dsZS5ocoILKi5nb29nbGUuaHSCCyouZ29vZ2xlLmh1
ggsqLmdvb2dsZS5pZYILKi5nb29nbGUuaW2CDSouZ29vZ2xlLmluZm+CCyouZ29v
Z2xlLmlxggsqLmdvb2dsZS5pc4ILKi5nb29nbGUuaXSCDiouZ29vZ2xlLml0LmFv
ggsqLmdvb2dsZS5qZYILKi5nb29nbGUuam+CDSouZ29vZ2xlLmpvYnOCCyouZ29v
Z2xlLmpwggsqLmdvb2dsZS5rZ4ILKi5nb29nbGUua2mCCyouZ29vZ2xlLmt6ggsq
Lmdvb2dsZS5sYYILKi5nb29nbGUubGmCCyouZ29vZ2xlLmxrggsqLmdvb2dsZS5s
dIILKi5nb29nbGUubHWCCyouZ29vZ2xlLmx2ggsqLmdvb2dsZS5tZIILKi5nb29n
bGUubWWCCyouZ29vZ2xlLm1nggsqLmdvb2dsZS5ta4ILKi5nb29nbGUubWyCCyou
Z29vZ2xlLm1uggsqLmdvb2dsZS5tc4ILKi5nb29nbGUubXWCCyouZ29vZ2xlLm12
ggsqLmdvb2dsZS5td4ILKi5nb29nbGUubmWCDiouZ29vZ2xlLm5lLmpwggwqLmdv
b2dsZS5uZXSCCyouZ29vZ2xlLm5sggsqLmdvb2dsZS5ub4ILKi5nb29nbGUubnKC
CyouZ29vZ2xlLm51gg8qLmdvb2dsZS5vZmYuYWmCCyouZ29vZ2xlLnBrggsqLmdv
b2dsZS5wbIILKi5nb29nbGUucG6CCyouZ29vZ2xlLnBzggsqLmdvb2dsZS5wdIIL
Ki5nb29nbGUucm+CCyouZ29vZ2xlLnJzggsqLmdvb2dsZS5ydYILKi5nb29nbGUu
cneCCyouZ29vZ2xlLnNjggsqLmdvb2dsZS5zZYILKi5nb29nbGUuc2iCCyouZ29v
Z2xlLnNpggsqLmdvb2dsZS5za4ILKi5nb29nbGUuc22CCyouZ29vZ2xlLnNuggsq
Lmdvb2dsZS5zb4ILKi5nb29nbGUuc3SCCyouZ29vZ2xlLnRkggsqLmdvb2dsZS50
Z4ILKi5nb29nbGUudGuCCyouZ29vZ2xlLnRsggsqLmdvb2dsZS50bYILKi5nb29n
bGUudG6CCyouZ29vZ2xlLnRvggsqLmdvb2dsZS50cIILKi5nb29nbGUudHSCCyou
Z29vZ2xlLnVzggsqLmdvb2dsZS51eoILKi5nb29nbGUudmeCCyouZ29vZ2xlLnZ1
ggsqLmdvb2dsZS53c4IJZ29vZ2xlLmFjgglnb29nbGUuYWSCCWdvb2dsZS5hZYIJ
Z29vZ2xlLmFmgglnb29nbGUuYWeCCWdvb2dsZS5hbIIJZ29vZ2xlLmFtgglnb29n
bGUuYXOCCWdvb2dsZS5hdIIJZ29vZ2xlLmF6gglnb29nbGUuYmGCCWdvb2dsZS5i
ZYIJZ29vZ2xlLmJmgglnb29nbGUuYmeCCWdvb2dsZS5iaYIJZ29vZ2xlLmJqggln
b29nbGUuYnOCCWdvb2dsZS5ieYIJZ29vZ2xlLmNhggpnb29nbGUuY2F0gglnb29n
bGUuY2OCCWdvb2dsZS5jZIIJZ29vZ2xlLmNmgglnb29nbGUuY2eCCWdvb2dsZS5j
aIIJZ29vZ2xlLmNpgglnb29nbGUuY2yCCWdvb2dsZS5jbYIJZ29vZ2xlLmNuggxn
b29nbGUuY28uYW+CDGdvb2dsZS5jby5id4IMZ29vZ2xlLmNvLmNrggxnb29nbGUu
Y28uY3KCDGdvb2dsZS5jby5odYIMZ29vZ2xlLmNvLmlkggxnb29nbGUuY28uaWyC
DGdvb2dsZS5jby5pbYIMZ29vZ2xlLmNvLmluggxnb29nbGUuY28uamWCDGdvb2ds
ZS5jby5qcIIMZ29vZ2xlLmNvLmtlggxnb29nbGUuY28ua3KCDGdvb2dsZS5jby5s
c4IMZ29vZ2xlLmNvLm1hggxnb29nbGUuY28ubXqCDGdvb2dsZS5jby5ueoIMZ29v
Z2xlLmNvLnRoggxnb29nbGUuY28udHqCDGdvb2dsZS5jby51Z4IMZ29vZ2xlLmNv
LnVrggxnb29nbGUuY28udXqCDGdvb2dsZS5jby52ZYIMZ29vZ2xlLmNvLnZpggxn
b29nbGUuY28uemGCDGdvb2dsZS5jby56bYIMZ29vZ2xlLmNvLnp3gg1nb29nbGUu
Y29tLmFmgg1nb29nbGUuY29tLmFngg1nb29nbGUuY29tLmFpgg1nb29nbGUuY29t
LmFygg1nb29nbGUuY29tLmF1gg1nb29nbGUuY29tLmJkgg1nb29nbGUuY29tLmJo
gg1nb29nbGUuY29tLmJugg1nb29nbGUuY29tLmJvgg1nb29nbGUuY29tLmJygg1n
b29nbGUuY29tLmJ5gg1nb29nbGUuY29tLmJ6gg1nb29nbGUuY29tLmNugg1nb29n
bGUuY29tLmNvgg1nb29nbGUuY29tLmN1gg1nb29nbGUuY29tLmN5gg1nb29nbGUu
Y29tLmRvgg1nb29nbGUuY29tLmVjgg1nb29nbGUuY29tLmVngg1nb29nbGUuY29t
LmV0gg1nb29nbGUuY29tLmZqgg1nb29nbGUuY29tLmdlgg1nb29nbGUuY29tLmdo
gg1nb29nbGUuY29tLmdpgg1nb29nbGUuY29tLmdygg1nb29nbGUuY29tLmd0gg1n
b29nbGUuY29tLmhrgg1nb29nbGUuY29tLmlxgg1nb29nbGUuY29tLmptgg1nb29n
bGUuY29tLmpvgg1nb29nbGUuY29tLmtogg1nb29nbGUuY29tLmt3gg1nb29nbGUu
Y29tLmxigg1nb29nbGUuY29tLmx5gg1nb29nbGUuY29tLm10gg1nb29nbGUuY29t
Lm14gg1nb29nbGUuY29tLm15gg1nb29nbGUuY29tLm5hgg1nb29nbGUuY29tLm5m
gg1nb29nbGUuY29tLm5ngg1nb29nbGUuY29tLm5pgg1nb29nbGUuY29tLm5wgg1n
b29nbGUuY29tLm5ygg1nb29nbGUuY29tLm9tgg1nb29nbGUuY29tLnBhgg1nb29n
bGUuY29tLnBlgg1nb29nbGUuY29tLnBogg1nb29nbGUuY29tLnBrgg1nb29nbGUu
Y29tLnBsgg1nb29nbGUuY29tLnBygg1nb29nbGUuY29tLnB5gg1nb29nbGUuY29t
LnFhgg1nb29nbGUuY29tLnJ1gg1nb29nbGUuY29tLnNhgg1nb29nbGUuY29tLnNi
gg1nb29nbGUuY29tLnNngg1nb29nbGUuY29tLnNsgg1nb29nbGUuY29tLnN2gg1n
b29nbGUuY29tLnRqgg1nb29nbGUuY29tLnRugg1nb29nbGUuY29tLnRygg1nb29n
bGUuY29tLnR3gg1nb29nbGUuY29tLnVhgg1nb29nbGUuY29tLnV5gg1nb29nbGUu
Y29tLnZjgg1nb29nbGUuY29tLnZlgg1nb29nbGUuY29tLnZugglnb29nbGUuY3aC
CWdvb2dsZS5jeoIJZ29vZ2xlLmRlgglnb29nbGUuZGqCCWdvb2dsZS5ka4IJZ29v
Z2xlLmRtgglnb29nbGUuZHqCCWdvb2dsZS5lZYIJZ29vZ2xlLmVzgglnb29nbGUu
ZmmCCWdvb2dsZS5mbYIJZ29vZ2xlLmZygglnb29nbGUuZ2GCCWdvb2dsZS5nZYIJ
Z29vZ2xlLmdngglnb29nbGUuZ2yCCWdvb2dsZS5nbYIJZ29vZ2xlLmdwgglnb29n
bGUuZ3KCCWdvb2dsZS5neYIJZ29vZ2xlLmhrgglnb29nbGUuaG6CCWdvb2dsZS5o
coIJZ29vZ2xlLmh0gglnb29nbGUuaHWCCWdvb2dsZS5pZYIJZ29vZ2xlLmltggtn
b29nbGUuaW5mb4IJZ29vZ2xlLmlxgglnb29nbGUuaXOCCWdvb2dsZS5pdIIMZ29v
Z2xlLml0LmFvgglnb29nbGUuamWCCWdvb2dsZS5qb4ILZ29vZ2xlLmpvYnOCCWdv
b2dsZS5qcIIJZ29vZ2xlLmtngglnb29nbGUua2mCCWdvb2dsZS5reoIJZ29vZ2xl
Lmxhgglnb29nbGUubGmCCWdvb2dsZS5sa4IJZ29vZ2xlLmx0gglnb29nbGUubHWC
CWdvb2dsZS5sdoIJZ29vZ2xlLm1kgglnb29nbGUubWWCCWdvb2dsZS5tZ4IJZ29v
Z2xlLm1rgglnb29nbGUubWyCCWdvb2dsZS5tboIJZ29vZ2xlLm1zgglnb29nbGUu
bXWCCWdvb2dsZS5tdoIJZ29vZ2xlLm13gglnb29nbGUubmWCDGdvb2dsZS5uZS5q
cIIKZ29vZ2xlLm5ldIIJZ29vZ2xlLm5sgglnb29nbGUubm+CCWdvb2dsZS5ucoIJ
Z29vZ2xlLm51gg1nb29nbGUub2ZmLmFpgglnb29nbGUucGuCCWdvb2dsZS5wbIIJ
Z29vZ2xlLnBugglnb29nbGUucHOCCWdvb2dsZS5wdIIJZ29vZ2xlLnJvgglnb29n
bGUucnOCCWdvb2dsZS5ydYIJZ29vZ2xlLnJ3gglnb29nbGUuc2OCCWdvb2dsZS5z
ZYIJZ29vZ2xlLnNogglnb29nbGUuc2mCCWdvb2dsZS5za4IJZ29vZ2xlLnNtggln
b29nbGUuc26CCWdvb2dsZS5zb4IJZ29vZ2xlLnN0gglnb29nbGUudGSCCWdvb2ds
ZS50Z4IJZ29vZ2xlLnRrgglnb29nbGUudGyCCWdvb2dsZS50bYIJZ29vZ2xlLnRu
gglnb29nbGUudG+CCWdvb2dsZS50cIIJZ29vZ2xlLnR0gglnb29nbGUudXOCCWdv
b2dsZS51eoIJZ29vZ2xlLnZngglnb29nbGUudnWCCWdvb2dsZS53czANBgkqhkiG
9w0BAQUFAAOBgQCROJdKT00d96BpNG4j3Xf5Kz7kJENMTYtgsGQW5E6y2yjRaguD
LPO+y4IH9KiVXD+qO8koye9yOMNawN9r/DFQd+t2nDmvlpcwJBNguiuqxl+rJaU8
KKgswikGaaM4z+i4vHuXcCKZtM/ELAaJlSaBPip4GBAkgv7D9hwh+sWvYA==
-----END CERTIFICATE-----";

        [Fact]
        public void A()
        {
            var x509 = new X509Certificate2(Encoding.ASCII.GetBytes(certificateData));

            Assert.Equal("CN=test-dir, O=Golang Tests, S=Test-State, C=UK", x509.SubjectName.Name);
            Assert.Equal("CN=test-dir, O=Golang Tests, S=Test-State, C=UK", x509.Issuer);
            Assert.Equal("1/30/2027 3:50:27 PM", x509.GetExpirationDateString());

            var expires = new DateTime(2027, 1, 30, 15, 50, 27, 0, DateTimeKind.Utc);

            Uid privateKeyId = Guid.NewGuid();

            var certificate = new CertificateInfo(
                id                  : 1,
                ownerId            : 93,
                parentId            : 1835,
                encryptedPrivateKey : null,
                data                : x509.GetRawCertData(),
                expires             : expires
            );

            var names = x509.GetAlternateSubjectNames().ToArray();

            Assert.Empty(names);
            Assert.Equal(1,            certificate.Id);
            Assert.Equal(expires,      certificate.Expires);
            Assert.Equal(93,           certificate.IssuerId);
            Assert.Equal(1835,         certificate.ParentId);

            var x5092 = new X509Certificate2(certificate.Data);

            Assert.Equal(expires, x5092.NotAfter);

            Assert.Null(certificate.Revoked);
        }


        [Fact]
        public void B()
        {
            var cert = new X509Certificate2(Encoding.ASCII.GetBytes(certificateData2));

            Assert.Equal("CN=google.com, O=Google Inc, L=Mountain View, S=California, C=US", cert.SubjectName.Name);

            var subjects = cert.GetAlternateSubjectNames().ToArray();

            /*
            DNS Name=google.com
            DNS Name=*.google.com
            DNS Name=*.youtube.com
            DNS Name=youtube.com
            DNS Name=*.youtube-nocookie.com
            DNS Name=youtu.be
            ...
            */

            Assert.Equal(458, subjects.Length);

            foreach (var name in subjects)
            {
                Assert.Equal("DNS Name", name.Type);
            }

            Assert.Equal("google.com",    subjects[0].Name);
            Assert.Equal("*.google.com",  subjects[1].Name);
            Assert.Equal("*.youtube.com", subjects[2].Name);
        }


       

        /*
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
        */
        }
    }