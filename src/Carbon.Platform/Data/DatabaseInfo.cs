using System;

using Carbon.Data.Annotations;

namespace Carbon.Platform.Data
{
    [Dataset("Databases")]
    public class DatabaseInfo : IDatabase
    {
        public DatabaseInfo() { }

        public DatabaseInfo(long id, string name)
        {
            Id   = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        [Member("id"), Key]
        public long Id { get; }

        [Member("name"), Unique]
        public string Name { get; }

        [Member("created"), Timestamp]
        public DateTime Created { get; }
    }
}