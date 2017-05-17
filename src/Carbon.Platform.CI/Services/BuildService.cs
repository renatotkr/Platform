using System;
using System.Threading.Tasks;
using Carbon.Extensions;
using Carbon.Platform.VersionControl;

using Dapper;

namespace Carbon.Platform.CI
{
    public class BuildService
    {
        private readonly PlatformDb db;
        private IRepositoryService repositoryService;

        public BuildService(PlatformDb db, IRepositoryService repositoryService)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
            this.repositoryService = repositoryService ?? throw new ArgumentNullException(nameof(repositoryService));
        }

        public async Task<Build> CreateAsync(CreateBuildRequest request)
        {
            if (request.Revision.Length != 40)
            {
                throw new Exception("Must be a sha1 commit");
            }

            var commit = await repositoryService.GetCommitAsync(
                repositoryId : request.RepositoryId,
                sha          : HexString.ToBytes(request.Revision)
            ).ConfigureAwait(false);

            // var commit = repositoryService.CreateCommitAsync(new CreateCommitRequest())
            var id = db.Builds.Sequence.Next();
            
            var build = new Build(
                id          : id,
                creatorId   : request.CreatorId,
                commitId    : commit.Id,
                resource    : request.Resource
             );

            await db.Builds.InsertAsync(build).ConfigureAwait(false);

            return build;
        }

        public async Task UpdateAsync(Build build)
        {
            // status, message, completed

            using (var connection = db.Context.GetConnection())
            {
                await connection.ExecuteAsync(
                    @"UPDATE `Builds`
                      SET `completed` = @completed,
                          `status` = @status,
                          `message` = @message,
                          `duration` = @duration
                       WHERE `id` = @id", build).ConfigureAwait(false);
            }
        }
    }
}