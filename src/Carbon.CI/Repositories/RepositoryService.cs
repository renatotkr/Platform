﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Data.Expressions;
using Carbon.Platform.Resources;

namespace Carbon.CI
{
    using static Expression;

    public class RepositoryService : IRepositoryService
    {
        private readonly CiadDb db;
        private readonly RepositoryBranchService branchService;

        public RepositoryService(CiadDb db)
        {
            this.db            = db ?? throw new ArgumentNullException(nameof(db));
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

        public async Task<RepositoryInfo> CreateAsync(CreateRepositoryRequest request)
        {
            #region Preconditions

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            #endregion

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
            await branchService.CreateAsync(
                new CreateBranchRequest(repository.Id, "master", request.OwnerId)
            );

            return repository;
        }
    }
}