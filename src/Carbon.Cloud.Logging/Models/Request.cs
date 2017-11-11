using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Data.Sequences;

namespace Carbon.Cloud.Logging
{
    [Dataset("Requests", Schema = "Logs")]
    [DataContract]
    public class Request : IRequest
    {
        public Request() { }

        public Request(
            Uid id,
            long environmentId,
            long domainId,
            string path,
            HttpMethod method               = HttpMethod.Get,
            HttpProtocol protocol           = HttpProtocol.Http1,
            int status                      = 200,
            long? serverId                  = null,
            Uid? clientId                   = null,
            string clientLocation           = null,
            EdgeCacheStatus edgeCacheStatus = default,
            string edgeLocation             = null,
            long? sessionId                 = null,
            long? securityTokenId           = null,
            long? programId                 = null,
            string programVersion           = null,
            long? originId                  = null,
            string referrer                 = null,
            long sentBytes                  = 0,
            long receivedBytes              = 0,
            TimeSpan responseTime           = default, // 0
            Uid? parentId                   = null)
        {
            Id              = id;
            EnvironmentId   = environmentId;
            DomainId        = domainId;
            Path            = path ?? throw new ArgumentNullException(nameof(path));
            Method          = method;
            Protocol        = protocol;
            ResponseTime    = responseTime;
            ParentId        = parentId;
            Status          = status;
            Referrer        = referrer;
            ClientId        = clientId;
            ClientLocation  = clientLocation;
            EdgeCacheStatus = edgeCacheStatus;
            EdgeLocation    = edgeLocation;
            OriginId        = originId;
            SessionId       = sessionId;
            SecurityTokenId = securityTokenId;
            ServerId        = serverId;
            ProgramId       = programId;
            ProgramVersion  = programVersion;
            SentBytes       = sentBytes;
            ReceivedBytes   = receivedBytes;
        }
        
        [Member("id"), Key]
        [DataMember(Order = 1)]
        public Uid Id { get; }

        [Member("environmentId")]
        [DataMember(Order = 2)]
        public long EnvironmentId { get; }

        [Member("domainId")]
        [DataMember(Order = 3)]
        public long DomainId { get; }

        [Member("path")]
        [StringLength(2083)]
        [DataMember(Order = 4)]
        public string Path { get; }
        
        [Member("method")]
        [DataMember(Order = 5)]
        public HttpMethod Method { get; }
        
        [Member("protocol", Order = 6)]
        public HttpProtocol Protocol { get; }

        [Member("status")]
        [DataMember(Order = 6)]
        public int Status { get; set; }
       
        [Member("referrer")]
        [StringLength(2083)]
        [DataMember(Order = 7, EmitDefaultValue = false)]
        public string Referrer { get; }

        [Member("originDomainId")]
        [DataMember(Order = 9, EmitDefaultValue = false)]
        public long? OriginDomainId { get; } // protocol + hostName + : + port
        
        // CipherId
    
        #region Client / Session / SecurityToken / Location (10-20)

        [Member("clientId")]
        [DataMember(Order = 10)]
        public Uid? ClientId { get; }

        [Member("clientLocation")] // e.g. US/NY/New`York
        [StringLength(255)]
        [DataMember(Order = 11, EmitDefaultValue = false)]
        public string ClientLocation { get; }
        
        [Member("sessionId")]
        [DataMember(Order = 12)]
        public long? SessionId { get; }

        [Member("securityTokenId")]
        [DataMember(Order = 13)]
        public long? SecurityTokenId { get; }
                
        #endregion

        #region Resource Usage (20-22)

        [Member("computeUnits")]
        [DataMember(Order = 20)]
        public long ComputeUnits { get; set; }

        [Member("receivedBytes")]
        [DataMember(Order = 21)]
        public long ReceivedBytes { get; set; }

        [Member("sentBytes")]
        [DataMember(Order = 22)]
        public long SentBytes { get; set; }

        #endregion

        #region Edge / Origin (30 - 32)
        
        // dc11-up-a227
        // ch1-up-a240
        
        [Member("edgeLocation")]
        [DataMember(Order = 30, EmitDefaultValue = false)]
        [StringLength(100)]
        public string EdgeLocation { get; }

        [Member("edgeCacheStatus")]
        [DataMember(Order = 31, EmitDefaultValue = false)]
        public EdgeCacheStatus EdgeCacheStatus { get; }

        [Member("originId")]
        [DataMember(Order = 32, EmitDefaultValue = false)]
        public long? OriginId { get; }

        #endregion

        #region Server, Origin, & Program (33-39)

        [Member("serverId")] // null if served by the edge
        [DataMember(Order = 33, EmitDefaultValue = false)]
        public long? ServerId { get; }
        
        [Member("programId")]
        [DataMember(Order = 34)]
        public long? ProgramId { get; }

        [Member("programVersion")]
        [Ascii, StringLength(100)]
        [DataMember(Order = 35, EmitDefaultValue = false)]
        public string ProgramVersion { get; }

        #endregion
        
        /// <summary>
        /// The total time spent servicing the request
        /// </summary>
        [Member("responseTime")]
        [DataMember(Order = 40, EmitDefaultValue = false)]
        public TimeSpan ResponseTime { get; set; }
        
        [DataMember(Order = 41, EmitDefaultValue = false)]
        public Timing[] Timings { get; set; }
        
        [Member("parentId")]
        [DataMember(Order = 50)]
        public Uid? ParentId { get; }
     
        [IgnoreDataMember]
        public DateTime Timestamp => RequestId.GetTimestamp(Id);
    }
}