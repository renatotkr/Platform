namespace Carbon.Platform
{
    public interface IProgram
    {
        long Id { get; }

        string Slug { get; }

        Semver Version { get; }
    }
}