using System;

using Carbon.Data.Annotations;

namespace Carbon.Platform.Data
{
    [Dataset("DatabaseEndpoints")]
    public class DatabaseEndpoint : IDatabaseEndpoint
    {
        public DatabaseEndpoint() { }

        public DatabaseEndpoint(
            long id, 
            string host,
            ILocation location,
            ushort port = 3306,
            DatabaseEndpointFlags flags = DatabaseEndpointFlags.None
        )
        {
            #region Preconditions

            if (location == null)
                throw new ArgumentNullException(nameof(location));

            #endregion

            Id    = id;
            Host  = host ?? throw new ArgumentNullException(nameof(host));
            Port  = port;
            Flags = flags;
            LocationId = location.Id;
        }

        // DatabaseId + Sequence
        [Member("id"), Key]
        public long Id { get; }

        [Member("host")]
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

    [Flags]
    public enum DatabaseEndpointFlags
    {
        None     = 0,
        Primary  = 1 << 0,
        ReadOnly = 1 << 4
    }
}