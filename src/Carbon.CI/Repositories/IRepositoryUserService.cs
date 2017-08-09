using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Security;

namespace Carbon.CI
{
    public interface IRepositoryUserService
    {
        Task<IReadOnlyList<RepositoryUser>> ListAsync(IRepository repository);

        Task<RepositoryUser> CreateAsync(CreateRepositoryUserRequest request, ISecurityContext context);
    }
}