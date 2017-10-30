using System;

using Carbon.Data.Annotations;
using Carbon.Data.Sequences;

namespace Carbon.Cloud.Logging
{
    [Dataset("Requests", Schema = "Logs")]
    public class Request // : IRequest
    {
        public Request() { }

        public Request(
            Uid id,
            long environmentId,
            long domainId,
            string path,
            string method,
            long serverId,
            Uid clientId,
            long programId,
            string programVersion = null,
            long? sessionId = null,
            long? originId = null,
            string referrer = null,
            TimeSpan computeTime = default, // 0
            Uid? parentId = null,
            int status = 200)
        {
            Id             = id;
            EnvironmentId  = environmentId;
            DomainId       = domainId;
            Path           = path ?? throw new ArgumentNullException(nameof(path));
            Method         = method;
            ServerId       = serverId;
            ProgramId      = programId;
            ProgramVersion = programVersion;
            ParentId       = parentId;
            Status         = status;
            ComputeTime    = computeTime;
            Referrer       = referrer;
            ClientId       = clientId;
            OriginId       = originId;
            SessionId      = sessionId;
        }
        
        [Member("id"), Key]
        public Uid Id { get; }

        [Member("environmentId")]
        public long EnvironmentId { get; }

        [Member("domainId")] 
        public long DomainId { get; }

        [Member("path")]
        [StringLength(2083)]
        public string Path { get; }
        
        [Member("method")]
        [Ascii, StringLength(20)]
        public string Method { get; }
        
        [Member("status")]
        public int Status { get; set; }
        
        [Member("parentId")]
        public Uid? ParentId { get; }

        [Member("programId")]
        public long ProgramId { get; set; }

        [Member("programVersion")]
        [StringLength(100)]
        public string ProgramVersion { get; set; }
        
        [Member("serverId")]
        public long? ServerId { get; }

        [Member("originId")]
        public long? OriginId { get; }

        #region Context

        [Member("sessionId")]
        public long? SessionId { get; }

        [Member("tokenId")]
        public long? TokenId { get; }

        [Member("clientId")]
        public Uid ClientId { get; }
        
        [Member("referrer")]
        [StringLength(2083)]
        public string Referrer { get; }

        [Member("origin")] // protocol + hostName + : + port
        [Ascii, StringLength(8  + 253 + 6)]
        public string Origin { get; }

        #endregion

        #region Resource Usage

        [Member("computeUnits")]
        public long ComputeUnits { get; set; }

        [Member("receivedBytes")]
        public long ReceivedBytes { get; set; }

        [Member("sentBytes")]
        public long SentBytes { get; set; }

        #endregion

        #region Timings

        // WaitTime?

        /// <summary>
        /// The time the CPU was spent processing the request
        /// </summary>
        [Member("computeTime")]
        public TimeSpan ComputeTime { get; set; }

        /// <summary>
        /// The total time spent servicing the request
        /// </summary>
        [Member("responseTime")]
        public TimeSpan ResponseTime { get; set; }

        #endregion

        #region Helpers

        public DateTime Created => RequestId.GetTimestamp(Id);

        public RequestTiming[] Timings { get; set; }

        #endregion
    }

    // BlobHits (BlobId, RequestId)
}