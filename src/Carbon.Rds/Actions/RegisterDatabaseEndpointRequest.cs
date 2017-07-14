using System;

using Carbon.Platform;
using Carbon.Platform.Storage;

namespace Carbon.Rds
{
    public class RegisterDatabaseEndpointRequest
    {
        public RegisterDatabaseEndpointRequest() { }

        public RegisterDatabaseEndpointRequest(string host, int port, DatabaseEndpointFlags flags, ILocation location = null)
        {
            #region Preconditions

            if (string.IsNullOrEmpty(host))
                throw new ArgumentException("Required", nameof(host));

            if (port <= 0)
                throw new ArgumentException("Must be > 0", nameof(port));

            #endregion

            Host       = host;
            Port       = port;
            Flags      = flags;
            LocationId = location?.Id;
        }

        public long DatabaseId { get; set; }
        
        public string Host { get; set; }

        public int Port { get; set; }

        public DatabaseEndpointFlags Flags { get; set; }

        public int? LocationId { get; set; }
    }
}
