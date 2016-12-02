namespace Carbon.Platform.Computing
{
    using Versioning;

    public interface IProgram
    {
        long Id { get; }

        SemanticVersion Version { get; }

        string Name { get; }
    }
}