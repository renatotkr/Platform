using System;
using Carbon.Platform.Resources;

namespace Carbon.Rds.Services
{
    public class RegisterDatabaseClusterRequest
    {
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

        public string Name { get; }

        public ManagedResource Resource { get; }

        public RegisterDatabaseInstanceRequest[] Instances { get; }

        public RegisterDatabaseEndpointRequest[] Endpoints { get; }
    }
}