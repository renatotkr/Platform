namespace Carbon.Platform
{
    public interface IProgram
    {
        long Id { get; }

        string Slug { get; }

        ProgramType Type { get; }
    }
}