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
            #region PReconditions

            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Required", nameof(name));

            #endregion

            Name      = name;
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