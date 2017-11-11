using Carbon.Data.Sequences;
using Carbon.Json;

namespace Carbon.Platform.Diagnostics
{
    public interface IException
    {
        Uid Id { get; }

        string Type { get; }

        string Message { get; }

        JsonObject Properties { get; }

        long? IssueId { get; }
    }
}