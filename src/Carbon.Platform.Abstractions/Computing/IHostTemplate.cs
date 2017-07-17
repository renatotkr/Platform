using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    public interface IHostTemplate : IResource
    {
        string Name { get; }

        long ImageId { get; }

        long MachineTypeId { get; }

        // Script run at startup
        string StartupScript { get; }
    }
}

// aws | instance launch configuration
// gcp | instance templates
