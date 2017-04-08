using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Json;

namespace Carbon.Platform.Apps
{
    // An app enviroment may span multiple backends (i.e. regions)

    [Dataset("AppEnvironments")]
    public class AppEnvironment : IAppEnvironment
    {
        public AppEnvironment() { }

        public AppEnvironment(long id, string name, JsonObject variables, ManagedResource resource)
        {
            Id         = id;
            Name       = name ?? throw new ArgumentNullException(nameof(name));
            Variables  = variables;
            ResourceId = resource.Id;
            ProviderId = resource.Provider.Id;
        }

        // AppId + Sequence
        [Member("id"), Key]
        public long Id { get; }

        [Member("name")]
        [StringLength(63)]
        public string Name { get; }

        [Member("variables")]
        public JsonObject Variables { get; }
        
        // current revision
        // e.g. 1.0.0 | master

        [Member("revision")]
        public string Revision { get; set; }

        #region Repository Info

        [Member("repositoryId")]
        public long RepositoryId { get; set; }

        [Member("repositoryHead")]
        public string RepositoryHead { get; set; }

        #endregion
        
        #region IResource

        [Member("providerId")]
        public int ProviderId { get; }

        [Member("resourceId")]
        public string ResourceId { get; }

        // App Environments are global
        long IManagedResource.LocationId => 0;

        ResourceType IManagedResource.ResourceType => ResourceType.AppEnvironment;

        #endregion

        #region Timestamps

        [IgnoreDataMember]
        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [IgnoreDataMember]
        [Member("deleted")]
        public DateTime? Deleted { get; }

        [IgnoreDataMember]
        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        #endregion
        
        public long AppId => ScopedId.GetScope(Id);
    }
}

/*
{
  "port": 8003,
  "host": "carbonmade.com",
  "framework": "net461"
}
*/ 