using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Security;

namespace Carbon.CI
{
    public interface IRepositoryUserService
    {
        Task<IReadOnlyList<RepositoryUser>> ListAsync(IRepository repository);

        Task<IReadOnlyList<RepositoryUser>> ListHavingUserIdAsync(long userId);

        Task<RepositoryUser> CreateAsync(CreateRepositoryUserRequest request, ISecurityContext context);

        // DeleteAsync
    }

    // RepositoryGrants?

}