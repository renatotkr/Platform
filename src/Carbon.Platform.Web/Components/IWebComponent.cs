namespace Carbon.Platform.Web
{
    public interface IWebComponent
    {
        long Id { get; }

        string Name { get; }

        string Namespace { get; }
    }
}