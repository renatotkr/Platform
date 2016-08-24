namespace Carbon.Platform
{
    public interface IProgram
    {
        long Id { get; }        // 1

        Semver Version { get; } // 2

        string Name { get; }    // 3
    }
}