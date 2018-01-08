using System.Threading.Tasks;

namespace Carbon.Platform.Hosting
{
    public interface IDomainRegistrationService
    {
        Task<DomainRegistration> GetAsync(long id);

        Task<DomainRegistration> CreateAsync(CreateDomainRegistrationRequest request);

        Task UpdateAsync(UpdateDomainRegistrationRequest request);
    }
}