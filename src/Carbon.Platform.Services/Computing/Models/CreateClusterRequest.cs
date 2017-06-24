using System;

using Carbon.Json;

namespace Carbon.Platform.Computing
{
    public class CreateClusterRequest
    {
        public CreateClusterRequest() { }

        public CreateClusterRequest(
            IEnvironment environment, 
            ILocation location,
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
            Properties    = properties ?? new JsonObject();
        }

        public string Name { get; set; }

        public long EnvironmentId { get; set; }

        public int LocationId { get; set; }

        public long? HostTemplateId { get; set; }

        public long? HealthCheckId { get; set; }

        public JsonObject Properties { get; set; }
    }
}