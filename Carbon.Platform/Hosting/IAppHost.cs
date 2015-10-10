namespace Carbon.Platform.Hosting
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    // This could be IIS, Apache, Unicorn, or a Self Host

    public interface IAppHost
    {
        IEnumerable<AppInstance> Scan();

        AppInstance Find(string name); // TODO (id)

        void Create(IApp app);

        void Delete(IApp app);

        Task Deploy(IApp app, int version, Package package);

        Task<AppInstance> Activate(IApp app, int version);

        void Reload(IApp app);

        bool IsDeployed(IApp app, int version);
    }
}