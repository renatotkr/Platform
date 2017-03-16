using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;

namespace Carbon.Platform.Databases
{
    [Dataset("Databases")]
    public class DatabaseInfo
    {
        [Member("id"), Key]
        public long Id { get; set;  }

        [Member("name")]
        [Indexed]
        public string Name { get; set; }

        [Member("created"), Timestamp]
        public DateTime Created { get; set; }
    }
}

// e.g. (1, "Carbon", "amnz:database:345-234-5234234)