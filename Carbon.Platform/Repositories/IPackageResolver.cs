namespace Carbon.Platform
{
    using System.Threading.Tasks;

    public interface IPackageResolver
    {
        Task<Package> GetAsync(CodeSource source);
    }
}