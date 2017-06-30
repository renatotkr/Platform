using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Data.Expressions;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Storage
{
    using static Expression;

    public class RepositoryService : IRepositoryService
    {
        private readonly RepositoryDb db;

        public RepositoryService(RepositoryDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public Task<IReadOnlyList<RepositoryInfo>> ListAsync(long ownerId)
        {
            return db.Repositories.QueryAsync(
                expression : And(Eq("ownerId", ownerId), IsNull("deleted")),
                order      : Order.Ascending("name"),
                take       : 1000
            );
        }

        public async Task<RepositoryInfo> GetAsync(long id)
        {
            return await db.Repositories.FindAsync(id).ConfigureAwait(false) 
                ?? throw ResourceError.NotFound(ResourceTypes.Repository, id);
        }

        public async Task<RepositoryInfo> GetAsync(long ownerId, string name)
        {
            var repository = await db.Repositories.QueryFirstOrDefaultAsync(
                And(Eq("ownerId", ownerId), Eq("name", name))
            ).ConfigureAwait(false);

            if (repository == null)
            {
                throw ResourceError.NotFound(ResourceTypes.Repository, ownerId, name);
            }

            return repository;
        }

        public async Task<RepositoryInfo> GetAsync(ResourceProvider provider, string fullName)
        {
            var repository = await db.Repositories.QueryFirstOrDefaultAsync(
                And(Eq("providerId", provider.Id), Eq("fullName", fullName))
            ).ConfigureAwait(false);

            if (repository == null)
            {
                throw ResourceError.NotFound(provider, ResourceTypes.Repository, fullName);
            }

            return repository;
        }

        public async Task<RepositoryInfo> CreateAsync(CreateRepositoryRequest request)
        {
            #region Preconditions

            Validate.Object(request, nameof(request));

            #endregion

            var repository = new RepositoryInfo(
                id       : await db.Repositories.Sequence.NextAsync(),
                name     : request.Name,
                ownerId  : request.OwnerId,
                resource : request.Resource
            )
            {
                EncryptedToken = request.EncryptedToken
            };

            await db.Repositories.InsertAsync(repository).ConfigureAwait(false);

            // Recreate the master branch

            await CreateBranchAsync(new CreateBranchRequest(repository.Id, "master", request.OwnerId));

            return repository;
        }

        public async Task<RepositoryBranch> CreateBranchAsync(CreateBranchRequest request)
        {
            #region Preconditions

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            #endregion

            var branchId = await db.RepositoryBranches.GetNextScopedIdAsync(request.RepositoryId);

            var branch = new RepositoryBranch(
                id           : branchId,
                repositoryId : request.RepositoryId, 
                name         : request.Name, 
                creatorId    : request.CreatorId
            );

            await db.RepositoryBranches.InsertAsync(branch).ConfigureAwait(false);

            return branch;
        }

        public async Task<IReadOnlyList<RepositoryBranch>> ListBranchesAsync(long repositoryId)
        {
            var result = await db.RepositoryBranches.QueryAsync(Eq("repositoryId", repositoryId)).ConfigureAwait(false);

            return result;
        }

        public async Task<RepositoryBranch> GetBranchAsync(long id)
        {
            return await db.RepositoryBranches.FindAsync(id) ?? throw ResourceError.NotFound(ResourceTypes.RepositoryBranch, id);
        }

        public async Task<RepositoryBranch> GetBranchAsync(long repositoryId, string name)
        {
            return await db.RepositoryBranches.QueryFirstOrDefaultAsync(
                And(Eq("repositoryId", repositoryId), Eq("name", name))
            ).ConfigureAwait(false) ?? throw new ResourceNotFoundException($"repository:branch#{repositoryId}/{name}");
        }
    }
}