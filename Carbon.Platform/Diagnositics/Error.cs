using System;
using System.Net;

namespace Carbon.Platform
{
    using Data.Annotations;

    [Dataset("Error")]
    public class Error
    {
        [PartitionKey]
        public long ProgramId { get; set; }

        [Identity]
        public long Id { get; set; }
        
        public Semver Version { get; set; }

        public long HostId { get; set; }

        public string Type { get; set; }

        public string Message { get; set; }

        [Optional]
        public string StackTrace { get; set; }

        [Optional]
        public string InnerType { get; set; }

        [Optional]
        public string InnerMessage { get; set; }

        [Optional]
        public string InnerStackTrack { get; set; }

        [Optional]
        public string[] Tags { get; set; }

        #region Context

        [Optional]
        public string HttpMethod { get; set; }

        [Optional]
        public Uri Url { get; set; }

        [Optional]
        public IPAddress Ip { get; set; }

        [Optional]
        public string Referrer { get; set; }

        [Optional]
        [Indexed]
        public int? UserId { get; set; }

        [Optional]
        public long? ClientId { get; set; }

        [Optional]
        public string UserAgent { get; set; }

        #endregion

        #region Helpers

        public void SetException(Exception exception)
        {
            Type = exception.GetType().Name;
            Message = exception.Message;
            StackTrace = exception.StackTrace;

            if (exception.InnerException != null)
            {
                var inner = exception.InnerException;

                InnerType = inner.GetType().Name;
                InnerMessage = inner.Message;
                InnerStackTrack = inner.StackTrace;
            }
        }

        #endregion

        // [IgnoreDataMember]
        // public object User { get; set; }
    }
}