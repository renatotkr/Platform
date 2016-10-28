namespace Carbon.Computing
{
    public interface IProgram
    {
        long Id { get; }

        Semver Version { get; }

        string Name { get; }
    }
}