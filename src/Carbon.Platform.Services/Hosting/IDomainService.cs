using System.Threading.Tasks;

namespace Carbon.Platform.Hosting
{
    public interface IDomainService
    {
        Task<Domain> GetAsync(long id);

        Task<Domain> FindAsync(string name);

        Task<Domain> CreateAsync(CreateDomainRequest request);

        Task UpdateAsync(UpdateDomainRequest request);
    }
}