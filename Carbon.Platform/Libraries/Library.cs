using System;
using System.Collections.Generic;

namespace Carbon.Platform
{
    using Data.Annotations;

    [Record(TableName = "Libraries")]
    public class Library
    {
        [Member(1), Identity]
        public long Id { get; set; }

        [Member(2), Unique]
        public string Slug { get; set; }
        
        [Member(4)]
        public long RepositoryId { get; }

        [Member(5), Timestamp(false)]
        public DateTime Created { get; set; }
    }
}