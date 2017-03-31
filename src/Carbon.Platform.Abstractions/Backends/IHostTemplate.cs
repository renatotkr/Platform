namespace Carbon.Platform.Computing
{
    public interface IHostTemplate
    {
        long Id { get; }

        long MachineImageId { get; }

        long MachineTypeId { get; }

        string Script { get; }
    }
}