using System;

using Carbon.Data.Annotations;
using Carbon.Platform.Sequences;

namespace Carbon.Rds
{
    [Dataset("DatabaseSchemas", Schema = "Rds")]
    public class DatabaseSchema : IDatabaseSchema
    {
        public DatabaseSchema() { }

        public DatabaseSchema(long id, string name)
        {
            #region Preconditions

            if (id <= 0)
                throw new ArgumentException("Must be > 0", nameof(id));

            if (string.IsNullOrEmpty(nameof(name)))
                throw new ArgumentException("Required", nameof(name));

            #endregion

            Id   = id;
            Name = name;
        }

        // databaseId | #
        [Member("id"), Key]
        public long Id { get; }

        [Member("name"), Key]
        [StringLength(63)]
        public string Name { get; }

        #region Preconditions

        [Member("tableCount")]
        public int TableCount { get; }

        #endregion

        #region Timestamps

        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        [Member("deleted")]
        public DateTime? Deleted { get; }

        #endregion

        public long DatabaseId => ScopedId.GetScope(Id);
    }
}