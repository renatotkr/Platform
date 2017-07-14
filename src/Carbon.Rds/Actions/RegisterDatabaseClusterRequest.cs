using System;
using Carbon.Platform.Resources;

namespace Carbon.Rds
{
    public class RegisterDatabaseClusterRequest
    {
        public RegisterDatabaseClusterRequest() { }

        public RegisterDatabaseClusterRequest(
            string name,
            ManagedResource resource, 
            RegisterDatabaseInstanceRequest[] instances,
            RegisterDatabaseEndpointRequest[] endpoints)
        {
            Name      = name ?? throw new ArgumentNullException(nameof(name));
            Resource  = resource;
            Instances = instances;
            Endpoints = endpoints;
        }

        public long DatabaseId { get; set; }

        public string Name { get; set; }

        public ManagedResource Resource { get; set; }

        public RegisterDatabaseInstanceRequest[] Instances { get; set; }

        public RegisterDatabaseEndpointRequest[] Endpoints { get; set; }
    }
}