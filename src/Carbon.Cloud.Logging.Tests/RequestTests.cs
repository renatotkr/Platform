
using System;
using System.IO;

using Carbon.Data.Sequences;
using Carbon.Platform.Computing;

using ProtoBuf;

using Xunit;

namespace Carbon.Cloud.Logging
{
    public class RequestTests
    {
        [Fact]
        public void A()
        {
            var request = new Request(
                id              : Guid.NewGuid(),
                environmentId   : 1,
                domainId        : 2,
                path            : "/",
                method          : HttpMethod.Post,
                serverId        : HostId.Create(1, 1),
                programId       : 3,
                originId        : 10,
                referrer        : "https://google.com/",
                sessionId       : 456234,
                securityTokenId : 500,
                responseTime    : TimeSpan.FromSeconds(1.3)
            );

            Assert.Null(request.ClientId);

            var ms = new MemoryStream();

            var log = new RequestLog(ms);

            var count = 1_000_000;

            for (var i = 0; i < count; i++)
            {
                request.ComputeUnits = i;

                log.Append(request);
            }

            ms.Position = 0;

            var readCount = 0;

            // ~ 93MB per million requests

            // 0.02 cents per month per 10,000,000 requests
            
            // $0.0125 per GB

            // throw new Exception((ms.Length / (1000 * 1000)).ToString());

            while (log.TryRead(out var b))
            {
                Assert.Equal(readCount, b.ComputeUnits);

                readCount++;
            }

            Assert.Equal(count, readCount);
        }

        [Fact]
        public void FromBase64()
        {
            var text = "ChUIxO7h7rqev+w8EPa2tKmfqq2BrwEQARgCIgEvKAMwrQI6E2h0dHBzOi8vZ29vZ2xlLmNvbS9SBAgAEABaDlVTL05ZL05ld2BZb3JrYKrsG2j0A6gBZLABZPIBA2NoMfgBBIACCogCgYCAgBCQAgOaAgUxLjAuMMICBQiUHhAEygIVCgZkZWNvZGUSBAgCEAMaBQj6BhAEygIVCgZlbmNvZGUSBAgCEAMaBQioFBAE";

            var a = Convert.FromBase64String(text);

            // 177 bytes
            
            var request = Serializer.Deserialize<Request>(new MemoryStream(a));

            Assert.Equal(Uid.Parse("5dTcGYChmFCf1zMMuAp72C"), request.Id);
            Assert.Equal(1,                                   request.EnvironmentId);
            Assert.Equal(2,                                   request.DomainId);
            Assert.Equal(HttpMethod.Post,                     request.Method);
            Assert.Equal("/",                                 request.Path);
            Assert.Equal(4294967297,                          request.ServerId);
            Assert.Equal(3,                                   request.ProgramId);
            Assert.Equal("1.0.0",                             request.ProgramVersion);
            Assert.Equal(10,                                  request.OriginId);
            Assert.Equal("https://google.com/",               request.Referrer);
            Assert.Equal(1.93,                                request.ResponseTime.TotalSeconds);
            Assert.Equal(100,                                 request.ReceivedBytes);
            Assert.Equal(100,                                 request.SentBytes);
            Assert.Equal("US/NY/New`York",                    request.ClientLocation);
            Assert.Equal(EdgeCacheStatus.Revalidated,         request.EdgeCacheStatus);
            Assert.Equal("ch1",                               request.EdgeLocation);
            Assert.Equal(456234,                              request.SessionId);
            Assert.Equal(500,                                 request.SecurityTokenId);
            Assert.Equal("decode",                            request.Timings[0].Name);
            Assert.Equal(0.445,                               request.Timings[0].Duration.TotalSeconds);
            Assert.Equal("encode",                            request.Timings[1].Name);
            Assert.Equal(1,                                   request.Timings[1].Start.TotalSeconds);
            Assert.Equal(1.3,                                 request.Timings[1].Duration.TotalSeconds);
        }

        [Fact]
        public void SerializeTest()
        {
            Uid id = Uid.Parse("5dTcGYChmFCf1zMMuAp72C");

            var request = new Request(
                id              : id, 
                environmentId   : 1, 
                domainId        : 2, 
                path            : "/", 
                method          : HttpMethod.Post,
                edgeLocation    : "ch1",
                edgeCacheStatus : EdgeCacheStatus.Hit,
                serverId        : HostId.Create(1, 1), 
                clientId        : default(Uid),
                clientLocation  : "US/NY/New`York",
                programId       : 3,
                programVersion  : "1.0.0",
                originId        : 10, 
                referrer        : "https://google.com/",
                sessionId       : 456234,
                securityTokenId : 500,
                status          : 301,
                responseTime    : TimeSpan.FromSeconds(1.93),
                receivedBytes   : 100,
                sentBytes       : 54052
            );
            
            request.Timings = new[] {
                new Timing("decode", TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(0.445)),
                new Timing("encode", TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1.3))
            };

            var ms = new MemoryStream();

            Serializer.Serialize(ms, request);


            ms.Position = 0;
     
            var b = Serializer.Deserialize<Request>(ms);

            Assert.Equal(id,                    b.Id);
            Assert.Equal(1,                     b.EnvironmentId);
            Assert.Equal(2,                     b.DomainId);
            Assert.Equal(HttpMethod.Post,       b.Method);
            Assert.Equal("/",                   b.Path);
            Assert.Equal(4294967297,            b.ServerId);
            Assert.Equal(3,                     b.ProgramId);
            Assert.Equal("1.0.0",               b.ProgramVersion);
            Assert.Equal(10,                    b.OriginId);
            Assert.Equal("https://google.com/", b.Referrer);
            Assert.Equal(1.93,                  b.ResponseTime.TotalSeconds);
            Assert.Equal(100,                   b.ReceivedBytes);
            Assert.Equal(54052,                 b.SentBytes);
            Assert.Equal("US/NY/New`York",      b.ClientLocation);
            Assert.Equal(EdgeCacheStatus.Hit,   b.EdgeCacheStatus);
            Assert.Equal("ch1",                 b.EdgeLocation);
            Assert.Equal(456234,                b.SessionId);
            Assert.Equal(500,                   b.SecurityTokenId);
            Assert.Equal("decode",              b.Timings[0].Name);
            Assert.Equal(0.445,                 b.Timings[0].Duration.TotalSeconds);
            Assert.Equal("encode",              b.Timings[1].Name);
            Assert.Equal(1,                     b.Timings[1].Start.TotalSeconds);
            Assert.Equal(1.3,                   b.Timings[1].Duration.TotalSeconds);
        }
        
        [Fact]
        public void ConstructorDefaultsAreCorrect()
        {
            var a = new Request(default(Uid), 1, 2, "/");

            Assert.Equal(EdgeCacheStatus.Unknown, a.EdgeCacheStatus);
            Assert.Equal(HttpMethod.Get,          a.Method);
            Assert.Equal(HttpProtocol.Http1,      a.Protocol);
           
        }
        [Fact]
        public void ConstructorSetsCorrectProperties()
        {
            var a = new Request(default(Uid), 1, 2, "/", HttpMethod.Get, HttpProtocol.Http2, originId: 10);

            Assert.Equal(1,                  a.EnvironmentId);
            Assert.Equal(2,                  a.DomainId);
            Assert.Equal(HttpMethod.Get,     a.Method);
            Assert.Equal(HttpProtocol.Http2, a.Protocol);
            Assert.Equal("/",                a.Path);
            Assert.Equal(10,                 a.OriginId);

            Assert.Null(a.ProgramId);
            Assert.Null(a.ProgramVersion);
            Assert.Null(a.ServerId);
            Assert.Null(a.ClientId);
            Assert.Null(a.ClientLocation);
            Assert.Null(a.OriginDomainId);
        }
    }
}