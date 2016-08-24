namespace Carbon.Platform
{
    public interface IProgram
    {
        long Id { get; }

        string Name { get; }

        Semver Version { get; }
    }
}