using System;
using System.ComponentModel.DataAnnotations;

namespace Carbon.Platform
{
    using Data;
    using Data.Annotations;

    [Record(TableName = "FrontendFiles")]
    public class FrontendFileInfo
    {
        [Key]
        public long FrontendId { get; set; }

        [Key]
        public string Name { get; set; }

        public CryptographicHash Hash { get; set; }

        [Timestamp]
        public DateTime Timestamp { get; set; }
    }

    // history key = id + timestamp
}