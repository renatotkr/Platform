using System;

using Carbon.Json;

namespace Carbon.Platform.Computing
{
    public class CreateClusterRequest
    {
        public CreateClusterRequest() { }

        public CreateClusterRequest(string name, ILocation location)
        {
            Name     = name;
            Location = location;
        }

        public CreateClusterRequest(IEnvironment environment, ILocation location, JsonObject details = null)
        {
            Environment = environment ?? throw new ArgumentNullException(nameof(environment));
            Location    = location ?? throw new ArgumentNullException(nameof(location));
            Name        = environment.Name + "/" + location.Name;
            Details     = details;
        }

        public string Name { get; set; }

        public IEnvironment Environment { get; set; }

        public ILocation Location { get; set; }

        public JsonObject Details { get; set; }
    }
}