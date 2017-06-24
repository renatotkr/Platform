using Carbon.Versioning;

namespace Carbon.Platform.Computing
{
    public interface IProgram
    {
        long Id { get; }

        string Name { get; }

        SemanticVersion Version { get; }
        
        string[] Addresses { get; }

        string Runtime { get; }
    }
}

/*
There are four specialized types of programs: Apps, Sites, Services, and Tasks

ref: https://en.wikipedia.org/wiki/Computer_program
*/