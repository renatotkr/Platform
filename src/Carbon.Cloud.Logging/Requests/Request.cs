using System;

using Carbon.Data.Annotations;
using Carbon.Data.Sequences;
using Carbon.Security;

namespace Carbon.Cloud.Logging
{
    [Dataset("Requests", Schema = "Logging")]
    public class Request // : IRequest
    {
        public Request() { }

        public Request(
            Uid id,
            long siteId,
            string method,
            string path,
            long environmentId,
            long serverId,
            ISecurityContext context = null,
            TimeSpan computeTime = default(TimeSpan), // 0
            Uid? parentId = null,
            Uid? exceptionId = null,
            int status = 200)
        {
            Id            = id;
            SiteId        = siteId;
            Method        = method;
            Path          = path ?? throw new ArgumentNullException(nameof(path));
            ServerId      = serverId;
            ParentId      = parentId;
            Status        = status;
            EnvironmentId = environmentId;
            ComputeTime   = computeTime;
            ExceptionId   = exceptionId;

            if (context != null)
            {
                ClientId   = Uid.Deserialize(context.ClientId.ToByteArray());
                ClientHash = Logging.ClientHash.Compute(context);
            }
        }
        
        [Member("id"), Key]
        public Uid Id { get; }

        [Member("siteId")] 
        public long SiteId { get; }
        
        [Member("method")]
        [StringLength(10)]
        public string Method { get; }

        [Member("path")]
        [StringLength(2000)]
        public string Path { get; }

        [Member("status")]
        public int Status { get; set; }
        
        // is the siteId sufficient?
        [Member("environmentId")]
        public long EnvironmentId { get; }

        [Member("referrerId")]
        public long? ReferrerId { get; }

        [Member("blobId")] // if the response was a blob
        public Uid? BlobId { get; set; }

        [Member("parentId")]
        public Uid? ParentId { get; }
        
        [Member("exceptionId")]
        public Uid? ExceptionId { get; }        

        #region Resource Usage (Compute & Data Transfer)

        [Member("computeUnits")]
        public long ComputeUnits { get; set; }

        [Member("receivedBytes")]
        public long ReceivedBytes { get; set; }

        [Member("sentBytes")]
        public long SentBytes { get; set; }

        #endregion

        #region Server

        [Member("serverId")]
        public long? ServerId { get; }

        #endregion

        #region Context

        [Member("sessionId")]
        public long? SessionId { get; }

        [Member("tokenId")]
        public long? TokenId { get; }

        [Member("clientId")]
        public Uid ClientId { get; }
        
        // sha1(clientId, ip, userAgent)
        [Member("clientHash"), FixedSize(20)]
        public byte[] ClientHash { get; }

        #endregion

        #region Timings

        [Member("computeTime")]
        [TimePrecision(TimePrecision.Millisecond)]
        public TimeSpan ComputeTime { get; set; }

        [Member("responseTime")]
        [TimePrecision(TimePrecision.Millisecond)]
        public TimeSpan ResponseTime { get; set; }

        #endregion

        #region Helpers

        public DateTime Created => Id.GetTimestamp();

        public RequestTiming[] Timings { get; set; }

        #endregion
    }
}