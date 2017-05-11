using Carbon.Json;

namespace Carbon.Platform.Diagnostics
{
    public interface IException
    {
        // Details

        string Type { get; }

        string Message { get; }

        JsonObject Details { get; }

        // RequestId

        long? SessionId { get; }

        long? ClientId { get; }
    }
}


/*
{
  "method": string,
  "url": string,
  "userAgent": string,
  "referrer": string,
  "responseStatusCode": number,
  "remoteIp": string,
}
*/
