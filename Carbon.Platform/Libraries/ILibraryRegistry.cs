namespace Carbon.Libraries
{
    using System.Threading.Tasks;

    using Carbon.Platform;

    public interface ILibraryRegistry
    {
        Task<Library> FindAsync(Hash hash);

        Task<Library> FindAsync(string name, Semver version);

        Task<Library> FindAsync(string name, SemverRange range);
    }  
}