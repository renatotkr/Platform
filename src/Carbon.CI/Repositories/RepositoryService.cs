using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Cloud.Logging;
using Carbon.Data;
using Carbon.Data.Expressions;
using Carbon.Platform.Resources;
using Carbon.Security;

namespace Carbon.CI
{
    using static Expression;

    public class RepositoryService : IRepositoryService
    {
        private readonly CiadDb db;
        private readonly IRepositoryBranchService branchService;
        private readonly IEventLogger eventLog;

        public RepositoryService(CiadDb db, IEventLogger eventLog)
        {
            this.db            = db ?? throw new ArgumentNullException(nameof(db));
            this.eventLog      = eventLog ?? throw new ArgumentNullException(nameof(eventLog));
            this.branchService = new RepositoryBranchService(db);
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
            return await db.Repositories.FindAsync(id)
                ?? throw ResourceError.NotFound(ResourceTypes.Repository, id);
        }

        public Task<RepositoryInfo> FindAsync(long ownerId, string name)
        {
            return db.Repositories.QueryFirstOrDefaultAsync(
                And(Eq("ownerId", ownerId), Eq("name", name))
            );
        }

        public async Task<RepositoryInfo> GetAsync(long ownerId, string name)
        {
            return await FindAsync(ownerId, name)
                ?? throw ResourceError.NotFound(ResourceTypes.Repository, ownerId, name);
        }

        public async Task<RepositoryInfo> CreateAsync(CreateRepositoryRequest request, ISecurityContext context)
        {
            Validate.NotNull(request, nameof(request));
            Validate.NotNull(context, nameof(context));

            var repository = new RepositoryInfo(
                id                  : await db.Repositories.Sequence.NextAsync(),
                name                : request.Name,
                ownerId             : request.OwnerId,
                origin              : request.Origin,
                providerId          : request.ProviderId,
                encryptedAcessToken : request.EncryptedAccessToken,
                properties          : request.Properties
            );

            // TODO: Create the repository and it's first branch inside of a transaction

            await db.Repositories.InsertAsync(repository);

            // Create the master branch
            await branchService.CreateAsync(new CreateBranchRequest(repository.Id, "master"), context);

            #region Logging

            await eventLog.CreateAsync(new Event(
                action   : "create",
                resource : "repository#" + repository.Id,
                userId   : context.UserId.Value
            ));

            #endregion

            return repository;
        }

        public async Task<bool> DeleteAsync(IRepository repository)
        {
            Validate.NotNull(repository, nameof(repository));

            return await db.Repositories.PatchAsync(repository.Id, new[] {
                Change.Replace("deleted", Func("NOW"))
            }, condition: IsNull("deleted")) > 0;
        }
    }
}