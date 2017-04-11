namespace Carbon.Platform.Computing
{
    public interface IHostTemplate : IManagedResource
    {
        long Id { get; }

        long MachineImageId { get; }

        long MachineTypeId { get; }

        string Script { get; }
    }
}