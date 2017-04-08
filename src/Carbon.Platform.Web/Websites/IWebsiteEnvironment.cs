using Carbon.Json;

namespace Carbon.Platform.Web
{
    public interface IWebsiteEnvironment
    {
        long Id { get; }

        string Name { get; }

        long WebsiteId { get; }
            
        string ReleaseVersion { get; }

        long AppEnvironmentId { get; }
      
        JsonObject Variables { get; }
    }
}