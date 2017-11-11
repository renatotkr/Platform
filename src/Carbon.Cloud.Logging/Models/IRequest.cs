using System;

using Carbon.Data.Sequences;

namespace Carbon.Cloud.Logging
{
    public interface IRequest
    {
        Uid Id { get; }

        long EnvironmentId { get; }

        long DomainId { get; }

        string Path { get; }

        HttpMethod Method { get; }

        int Status { get; }

        string Referrer { get; }

        DateTime Timestamp { get; }
    }
}