namespace Carbon.Platform.Computing
{
    public interface IProgram
    {
        long Id { get; }

        string Name { get; }
    }
}

// There are three types of programs: Applications, Services, and Tasks

// /var/apps/accelerator
// /var/services/accelerator
// /var/tasks/cleanup

// https://en.wikipedia.org/wiki/Computer_program
