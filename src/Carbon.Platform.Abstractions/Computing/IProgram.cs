using Carbon.Versioning;

namespace Carbon.Platform.Computing
{
    public interface IProgram
    {
        long Id { get; }

        string Name { get; }

        SemanticVersion Version { get; }
    }
}

/*
There are three specialized types of programs: Applications, Services, and Tasks

ref: https://en.wikipedia.org/wiki/Computer_program
*/