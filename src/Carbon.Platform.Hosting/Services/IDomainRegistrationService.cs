using System.Threading.Tasks;

namespace Carbon.Platform.Hosting
{
    public interface IDomainRegistrationService
    {
        Task<DomainRegistration> CreateAsync(CreateDomainRegistrationRequest request);

        Task<DomainRegistration> GetAsync(long id);
    }
}