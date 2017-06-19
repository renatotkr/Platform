using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Platform.Sequences;

namespace Carbon.Platform.Storage
{
    [Dataset("DatabaseSchemas")]
    public class DatabaseSchema
    {
        public DatabaseSchema() { }

        public DatabaseSchema(long id, string name)
        {
            Id   = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        // databaseId + sequenceNumber
        [Member("id"), Key]
        public long Id { get; }

        [Member("name"), Key]
        [StringLength(63)]
        public string Name { get; }

        #region Timestamps

        [IgnoreDataMember]
        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [IgnoreDataMember]
        [Member("deleted")]
        public DateTime? Deleted { get; }

        #endregion

        public long DatabaseId => ScopedId.GetScope(Id);
    }
}