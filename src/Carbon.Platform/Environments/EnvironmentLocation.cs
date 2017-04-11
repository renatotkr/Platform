using Carbon.Data.Annotations;

namespace Carbon.Platform
{
    [Dataset("EnvironmentLocations")]
    public class EnvironmentLocation
    {
        public EnvironmentLocation(long environmentId, long locationId)
        {
            EnvironmentId = environmentId;
            LocationId = locationId;
        }

        [Member("environmentId"), Key]
        public long EnvironmentId { get; }
        
        [Member("locationId"), Key]
        public long LocationId { get; }
    }
}