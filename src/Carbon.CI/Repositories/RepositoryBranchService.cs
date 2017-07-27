using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data.Expressions;
using Carbon.Platform.Resources;
using Carbon.Security;

namespace Carbon.CI
{
    using static Expression;

    public class RepositoryBranchService : IRepositoryBranchService
    {
        private readonly CiadDb db;

        public RepositoryBranchService(CiadDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public Task<IReadOnlyList<RepositoryBranch>> ListAsync(IRepository repository)
        {
            return db.RepositoryBranches.QueryAsync(Eq("repositoryId", repository.Id));
        }

        public async Task<RepositoryBranch> GetAsync(long id)
        {
            return await db.RepositoryBranches.FindAsync(id)
                ?? throw ResourceError.NotFound(ResourceTypes.RepositoryBranch, id);
        }

        public async Task<RepositoryBranch> GetAsync(long repositoryId, string name)
        {
            return await db.RepositoryBranches.QueryFirstOrDefaultAsync(
                And(Eq("repositoryId", repositoryId), Eq("name", name))
            ) ?? throw new ResourceNotFoundException($"repository:branch#{repositoryId}/{name}");
        }

        public async Task<RepositoryBranch> CreateAsync(CreateBranchRequest request, ISecurityContext context)
        {
            #region Preconditions

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            #endregion

            var branchId = await BranchId.NextAsync(db.Context, request.RepositoryId);

            var branch = new RepositoryBranch(
                id           : branchId,
                repositoryId : request.RepositoryId,
                name         : request.Name,
                creatorId    : context.UserId.Value
            );

            await db.RepositoryBranches.InsertAsync(branch);

            return branch;
        }
    }
}