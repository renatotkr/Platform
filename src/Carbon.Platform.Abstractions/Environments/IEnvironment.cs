using Carbon.Platform.Resources;

namespace Carbon.Platform.Environments
{
    public interface IEnvironment : IResource
    {
        string Name { get; }

        long OwnerId { get; }
    }
}

// An environment spans one or cloud regions
// Enviromements service one or more programs (apps) from development through production

// All resources belong to an environment...

// accelerator#stable
// accelerator#beta