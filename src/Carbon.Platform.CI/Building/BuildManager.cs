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

        public BuildManager(
            CodeBuildClient builder, 
            PlatformDb db)
        {
            this.db      = db      ?? throw new ArgumentNullException(nameof(db));
            this.builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }


        public Task<Build> GetAsync(long id)
        {
            return db.Builds.FindAsync(id);
        }

        public async Task<Amazon.CodeBuild.Build> CheckStatusAsync(IBuild build)
        {
            var result = await builder.BatchGetBuildsAsync(new BatchGetBuildsRequest(build.ResourceId));

            var b = result.Builds[0];
          
            return b;
        }

        private static BuildStatus GetStatus(Amazon.CodeBuild.Build build)
        {

            switch (build.BuildStatus)
            {
                case "FAILED"      : return BuildStatus.Failed;
                case "FAULT"       : return BuildStatus.Failed;
                case "IN_PROGRESS" : return BuildStatus.Building;
                case "STOPPED"     : return BuildStatus.Pending;
                case "SUCCEEDED"   : return BuildStatus.Completed;
                case "TIMED_OUT"   : return BuildStatus.Failed;
            }

            throw new Exception("unexpected status:" + build.BuildStatus);
        }

        public async Task<Build> StartAsync(CreateBuildRequest request)
        {
            var repository = request.Source.Repository;

            var commit = request.Source.Commit;

            if (!repository.Details.TryGetValue(RepositoryProperties.BuildProjectName, out var buildProjectName))
            {
                throw new Exception($"repository#{repository.Id} does not have a {RepositoryProperties.BuildProjectName}");
            }

            var id = db.Builds.Sequence.Next();

            // code build also injects
            // CODEBUILD_BUILD_ID

            var startBuildRequest = new StartBuildRequest(buildProjectName) { 
                EnvironmentVariablesOverride = new[] {
                    new EnvironmentVariable("BUILD_ID", id.ToString())
                },
                SourceVersion = HexString.FromBytes(commit.Sha1)
            };

            if (request.Output != null)
            {
                startBuildRequest.ArtifactsOverride = new ProjectArtifacts {
                    Type      = "S3",
                    Location  = request.Output.BucketName,
                    Path      = request.Output.Path,
                    Name      = request.Output.Name ?? id.ToString(),
                    Packaging = "NONE", // otherwise ZIP
                };
            }

            var remoteBuild = (await builder.StartBuildAsync(startBuildRequest).ConfigureAwait(false)).Build;
            
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
                      WHERE `id` = @id", build
                ).ConfigureAwait(false);
            }
        }
    }
}
