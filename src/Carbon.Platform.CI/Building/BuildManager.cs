using System;
using System.Threading.Tasks;

using Amazon.CodeBuild;
using Amazon.Helpers;

using Carbon.Platform.Resources;
using Carbon.Platform.VersionControl;

using Dapper;

namespace Carbon.Platform.CI
{
    public class BuildManager
    {
        private readonly CodeBuildClient builder;
        private readonly PlatformDb db;
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IRepositoryService repositoryService;

        public BuildManager(
            CodeBuildClient builder, 
            PlatformDb db,
            IRepositoryFactory repositoryFactory,
            IRepositoryService repositoryService)
        {
            this.db      = db ?? throw new ArgumentNullException(nameof(db));
            this.builder = builder ?? throw new ArgumentNullException(nameof(builder));
            this.repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
            this.repositoryService = repositoryService;
        }

        public async Task CheckStatusAsync(IBuild build)
        {
            var result = await builder.BatchGetBuildsAsync(new BatchGetBuildsRequest(build.ResourceId));

            var cloudBuild = result.Builds[0];
        }

        public async Task<Build> CreateBuildAsync(CreateBuildRequest request)
        {
            var source = request.Source;

            var provider = ResourceProvider.Get(source.Provider.Id);

            var repository = await repositoryService.GetAsync(provider, source.AccountName + "/" + source.RepositoryName);

            
            // TODO: Decrypt the acess token...

            // Resolve to a commit

            // Get or create the commit...
            // - we should have recieved a PushEvent from GITHUB and already registered it...

            var buildId = db.Builds.Sequence.Next();

            var client = repositoryFactory.Get(repository);

            var commitInfo = await client.GetCommitAsync(source.Revision.Value);

            var commit = await repositoryService.GetCommitAsync(
                repositoryId : repository.Id,
                sha          : HexString.ToBytes(commitInfo.Id)
            ).ConfigureAwait(false);

            // var commit = repositoryService.CreateCommitAsync(new CreateCommitRequest())
            var id = db.Builds.Sequence.Next();

            var codeBuild = await builder.StartBuildAsync(new StartBuildRequest {
                ProjectName = repository.Details[RepositoryProperties.BuildProjectName],
                EnviromentVariablesOverride = new[] {
                    new EnvironmentVariable("BUILDID", buildId.ToString())
                },
                SourceVersion = commit.Id.ToString()
            });

            // arn:aws:codebuild:region-ID:account-ID:build

            var build = new Build(
                id        : id,
                creatorId : request.CreatorId,
                commitId  : commit.Id,
                resource  : ManagedResource.Build(Locations.Aws_US_East_1, codeBuild.Build.Id)
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

        // CompleteBuild

        // CheckStatus
    }
}
