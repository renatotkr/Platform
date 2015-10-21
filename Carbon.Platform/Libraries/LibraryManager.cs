namespace Carbon.Libraries
{
    using System.Threading.Tasks;

    using Carbon.Platform;

    public interface ILibraryManager
    {
        Task<Library> FindAsync(string name, Semver version);

        Task<Library> PublishAsync(Library library);
    }  
}