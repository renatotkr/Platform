using System;

using Carbon.Data.Annotations;
using Carbon.Platform;
using Carbon.Platform.Sequences;

namespace Carbon.Rds
{
    [Dataset("DatabaseEndpoints", Schema = "Rds")]
    public class DatabaseEndpoint : IDatabaseEndpoint
    {
        public DatabaseEndpoint() { }

        public DatabaseEndpoint(
            long id, 
            string host,
            ILocation location,
            ushort port = 3306,
            DatabaseEndpointFlags flags = default)
        {
            #region Preconditions

            if (id <= 0)
                throw new ArgumentException("Must be > 0", nameof(id));

            if (string.IsNullOrEmpty(host))
                throw new ArgumentException("Required", nameof(host));

            if (location == null)
                throw new ArgumentNullException(nameof(location));

            #endregion

            Id         = id;
            Host       = host;
            Port       = port;
            Flags      = flags;
            LocationId = location.Id;
        }

        // databaseId | #
        [Member("id"), Key]
        public long Id { get; }

        [Member("host")]
        [Ascii, StringLength(253)]
        public string Host { get; }

        [Member("port")]
        public ushort Port { get; }

        [Member("flags")]
        public DatabaseEndpointFlags Flags { get; }

        [Member("locationId")]
        public int LocationId { get; }
        
        #region Helpers

        public bool IsReadOnly => Flags.HasFlag(DatabaseEndpointFlags.ReadOnly);

        public long DatabaseId => ScopedId.GetScope(Id);

        #endregion

        public override string ToString()
        {
            return Host + ":" + Port.ToString();
        }
    }
}