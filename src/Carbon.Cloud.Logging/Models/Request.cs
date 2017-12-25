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
            string referrer                 = null,
            long requestSize                = 0,
            long responseSize               = 0,
            Uid? clientId                   = null,
            string clientLocation           = null,
            EdgeCacheStatus edgeCacheStatus = default,
            int? edgeLocationId             = null,
            long? sessionId                 = null,
            long? securityTokenId           = null,
            long? programId                 = null,
            string programVersion           = null,
            long? serverId                  = null,

            long? originId                  = null,
            TimeSpan responseTime           = default, // 0
            Uid? parentId                   = null)
        {
            // TODO: Verify id & path
            
            Id              = id;     
            EnvironmentId   = environmentId;
            DomainId        = domainId;
            Path            = path;
            Method          = method;
            Protocol        = protocol;
            ResponseTime    = responseTime;
            ParentId        = parentId;
            Status          = status;
            Referrer        = referrer;
            ClientId        = clientId;
            ClientLocation  = clientLocation;
            EdgeCacheStatus = edgeCacheStatus;
            EdgeLocationId  = edgeLocationId;
            OriginId        = originId;
            SessionId       = sessionId;
            SecurityTokenId = securityTokenId;
            ServerId        = serverId;
            ProgramId       = programId;
            ProgramVersion  = programVersion;
            ResponseSize    = responseSize;
            RequestSize     = requestSize;
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

        #region Client (Id, Location, SessionId, SecurityTokenId) [10-20]

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

        /// <summary>
        ///  Total bytes received (header + body)
        /// </summary>
        [Member("requestSize")]
        [DataMember(Order = 21)]
        public long RequestSize { get; set; }
        
        /// <summary>
        /// Total bytes sent (header + body)
        /// </summary>
        [Member("responseSize")]
        [DataMember(Order = 22)]
        public long ResponseSize { get; set; }

        #endregion

        #region Edge (30 - 32)

        // dc11-up-a227
        // ch1-up-a240

        // EdgeLocation was 30

        [Member("edgeCacheStatus")]
        [DataMember(Order = 31, EmitDefaultValue = false)]
        public EdgeCacheStatus EdgeCacheStatus { get; }

        [Member("edgeLocationId")] // take over 30 and break existing logs
        [DataMember(Order = 32, EmitDefaultValue = false)]
        public int? EdgeLocationId { get; }
        
        #endregion

        #region Server (33)

        [Member("serverId")] // null if served by the edge
        [DataMember(Order = 33, EmitDefaultValue = false)]
        public long? ServerId { get; }
        
        #endregion

        #region Program (34 & 35)

        [Member("programId")]
        [DataMember(Order = 34)]
        public long? ProgramId { get; }

        [Member("programVersion")]
        [Ascii, StringLength(50)]
        [DataMember(Order = 35, EmitDefaultValue = false)]
        public string ProgramVersion { get; }

        #endregion
        
        [Member("originId")] // the upstream origin
        [DataMember(Order = 36, EmitDefaultValue = false)] // was 31
        public long? OriginId { get; }

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
        
        #region Helpers

        [IgnoreDataMember]
        public DateTime Timestamp => RequestId.GetTimestamp(Id);

        // Used to mark a request as "processed" before storing, so it's not processed again
        [DataMember(Name = "flags", Order = 51, EmitDefaultValue = false)]
        public RequestFlags Flags { get; set; }

        #endregion
    }

    public enum RequestFlags
    {
        None      = 0,
        Processed = 1 << 0
    }
}
