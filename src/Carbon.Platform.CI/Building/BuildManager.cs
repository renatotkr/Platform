using System;
using System.Threading.Tasks;

using Amazon.CodeBuild;
using Amazon.Helpers;

using Carbon.Platform.Resources;
using Carbon.Platform.Storage;

using Dapper;

namespace Carbon.Platform.CI
{
    public class BuildManager
    {
        private readonly CodeBuildClient builder;
        private readonly PlatformDb db;
        private readonly IRepositoryService repositoryService;

        public BuildManager(
            CodeBuildClient builder, 
            PlatformDb db,
            IRepositoryService repositoryService)
        {
            this.db      = db      ?? throw new ArgumentNullException(nameof(db));
            this.builder = builder ?? throw new ArgumentNullException(nameof(builder));
            this.repositoryService = repositoryService;
        }

        public async Task CheckStatusAsync(IBuild build)
        {
            var result = await builder.BatchGetBuildsAsync(new BatchGetBuildsRequest(build.ResourceId));

            var cloudBuild = result.Builds[0];
        }

        public async Task<Build> StartAsync(CreateBuildRequest request)
        {            
            var repository = await repositoryService.GetAsync(request.Source.RepositoryId);

            var commit = request.Source.Commit;

            var buildId = db.Builds.Sequence.Next();

            if (!repository.Details.TryGetValue(RepositoryProperties.BuildProjectName, out var buildProjectName))
            {
                throw new Exception($"repository#{repository.Id} does not have an associated buildProjectName");
            }

            // var commit = repositoryService.CreateCommitAsync(new CreateCommitRequest())
            var id = db.Builds.Sequence.Next();

            // code build also injects
            // CODEBUILD_BUILD_ID

            var remoteBuild = (await builder.StartBuildAsync(new StartBuildRequest {
                ProjectName = repository.Details[RepositoryProperties.BuildProjectName],

                EnviromentVariablesOverride = new[] {
                    new EnvironmentVariable("BUILD_ID", buildId.ToString())
                },

                SourceVersion = HexString.FromBytes(commit.Sha1)
            })).Build;

            
            // arn:aws:codebuild:region-ID:account-ID:build

            var build = new Build(
                id        : id,
                creatorId : request.CreatorId,
                commitId  : commit.Id,
                resource  : ManagedResource.Build(Locations.Aws_US_East_1, remoteBuild.Id)
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
