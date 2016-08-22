﻿using System;

namespace Carbon.Platform
{
    using Data;
    using Data.Annotations;

    [Record(TableName = "FrontendFiles")]
    public class FrontendFileInfo
    {
        [Member(1), Key]
        public long FrontendId { get; set; }

        [Member(2), Key]
        public string Name { get; set; }

        [Member(3)] 
        public CryptographicHash Hash { get; set; }

        [Member(4), Version]
        public DateTime Timestamp { get; set; }
    }

    // history key = id + timestamp
}