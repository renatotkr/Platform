namespace Carbon.Computing
{
    public interface IProgram
    {
        long Id { get; }

        SemanticVersion Version { get; }

        string Name { get; }
    }
}