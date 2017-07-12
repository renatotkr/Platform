using Carbon.Platform.Resources;

namespace Carbon.Platform.Storage
{
    public interface IChannelInfo : IResource
    {
        string Name { get; }
    }
}

// A channel may be a firehose, SNS Topic, Kinesis Stream, etc
// A channel may have one or more consumers / subscribers

// RentitionPeriod
