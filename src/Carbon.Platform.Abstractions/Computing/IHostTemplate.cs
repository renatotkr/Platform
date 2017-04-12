using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    public interface IHostTemplate : IManagedResource
    {
        long MachineImageId { get; }

        long MachineTypeId { get; }

        string Script { get; }
    }
}