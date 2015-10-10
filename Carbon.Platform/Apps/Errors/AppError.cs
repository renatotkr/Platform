namespace Carbon.Platform
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;
    using System.Net;

    using Carbon.Data;


    [Table("AppErrors")]
    public class AppError : IAppError
    {
        private static readonly Sequence sequence = new Sequence(0, 0);

        public static Sequence Sequence => sequence;

        [Column("appId"), Key]
        public int AppId { get; set; }

        [Column("id"), Key]
        public long Id { get; set; }

        [Column("appVersion")]
        public int? AppVersion { get; set; }

        [Column("machineId")]
        public int? MachineId { get; set; }

        [Column("type")]
        [Required]
        public string Type { get; set; }

        [Column("message")]
        public string Message { get; set; }

        [Column("stack")]
        public string StackTrace { get; set; }

        [Column("innerType")]
        public string InnerType { get; set; }

        [Column("innerMessage")]
        public string InnerMessage { get; set; }

        [Column("innerStack")]
        public string InnerStackTrack { get; set; }

        [Column("tags")]
        public string[] Tags { get; set; }

        #region Context

        [Column("httpMethod")]
        public string HttpMethod { get; set; }

        [Column("url")]
        public Uri Url { get; set; }

        [Column("ip")]
        public IPAddress Ip { get; set; }

        [Column("referrer")]
        public string Referrer { get; set; }

        [Column("userId")]
        [Index("userId-index")]
        public int? UserId { get; set; }

        [Column("clientId")]
        public long? ClientId { get; set; }

        [Column("ua")]
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

        [IgnoreDataMember]
        public object User { get; set; }

        [IgnoreDataMember]
        public DateTime Date => DateTimeOffset.FromUnixTimeSeconds((int)(Id >> Sequence.TimestampLeftShift)).UtcDateTime;
    }
}