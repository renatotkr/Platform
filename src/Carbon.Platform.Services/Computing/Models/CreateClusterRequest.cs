using System;
using System.ComponentModel.DataAnnotations;
using Carbon.Json;

namespace Carbon.Platform.Computing
{
    public class CreateClusterRequest
    {
        public CreateClusterRequest() { }

        public CreateClusterRequest(string name, ILocation location)
        {
            Name     = name     ?? throw new ArgumentNullException(nameof(name));
            Location = location ?? throw new ArgumentNullException(nameof(location));
        }

        public CreateClusterRequest(
            IEnvironment environment, 
            ILocation location,
            JsonObject properties = null)
        {
            Environment = environment ?? throw new ArgumentNullException(nameof(environment));
            Location    = location    ?? throw new ArgumentNullException(nameof(location));
            Name        = environment.Name + "/" + location.Name;
            Properties  = properties ?? new JsonObject();
        }

        public string Name { get; set; }

        public IEnvironment Environment { get; set; }

        [Required]
        public ILocation Location { get; set; }

        public long? HostTemplateId { get; set; }

        public long? HealthCheckId { get; set; }

        public JsonObject Properties { get; set; }
    }
}