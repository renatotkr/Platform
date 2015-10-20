namespace Carbon.Platform
{
    using System.IO;
    using System.Threading.Tasks;

    public interface IFileSystem
    {
        Task CreateAsync(string name, Stream stream);
    }
}