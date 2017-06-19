using Carbon.CI;

namespace Carbon.Platform.Computing
{
    public interface IProgramRelease : IRelease
    {
        long ProgramId { get; }
    }
}