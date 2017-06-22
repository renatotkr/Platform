namespace Carbon.Platform.Computing
{
    public interface IApplication : IProgram
    {
        string[] Urls { get; }
    }
}