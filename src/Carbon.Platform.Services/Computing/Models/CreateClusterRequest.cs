using System;

using Carbon.Json;
using Carbon.Platform.Environments;

namespace Carbon.Platform.Computing
{
    public class CreateClusterRequest
    {
        public CreateClusterRequest(
            IEnvironment environment, 
            ILocation location,
            IHostTemplate hostTemplate,
            JsonObject properties = null)
        {
            #region Preconditions

            if (environment == null)
                throw new ArgumentNullException(nameof(environment));

            if (location == null)
                throw new ArgumentNullException(nameof(location));
            
            #endregion

            EnvironmentId = environment.Id;
            LocationId    = location.Id;
            Name          = environment.Name + "/" + location.Name;
            HostTemplate  = hostTemplate;
            Properties    = properties ?? new JsonObject();
        }

        public string Name { get; }

        public long EnvironmentId { get; }

        public int LocationId { get; }

        public IHostTemplate HostTemplate { get; }

        public long? HealthCheckId { get; set; }

        public JsonObject Properties { get; }
    }
}