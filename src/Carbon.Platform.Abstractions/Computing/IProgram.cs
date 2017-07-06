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