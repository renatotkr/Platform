using Carbon.Data.Sequences;
using Carbon.Json;

namespace Carbon.Platform.Diagnostics
{
    public interface IException
    {
        BigId Id { get; }

        string Type { get; }

        string Message { get; }

        JsonObject Properties { get; }

        long? IssueId { get; }
        
        long? SessionId { get; }

        long? ClientId { get; }
    }
}