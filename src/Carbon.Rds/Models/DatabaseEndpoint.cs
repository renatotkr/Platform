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
            Validate.Id(id);
            Validate.NotNull(host, nameof(host));
            Validate.NotNull(location, nameof(location));

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

        public bool IsReadOnly => (Flags & DatabaseEndpointFlags.ReadOnly) != 0;

        public long DatabaseId => ScopedId.GetScope(Id);

        #endregion

        public override string ToString()
        {
            return Host + ":" + Port.ToString();
        }
    }
}