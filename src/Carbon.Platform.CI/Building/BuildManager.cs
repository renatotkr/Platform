using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Amazon.CodeBuild;
using Amazon.Helpers;
using Carbon.Data;
using Carbon.Data.Expressions;
using Carbon.Platform.Resources;
using Carbon.Platform.Sequences;

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

        public async Task<Build> GetLatestAsync(long projectId)
        {
            var range = ScopedId.GetRange(projectId);

            var builds = await db.Builds.QueryAsync(
                expression: Expression.Between("id", range.Start, range.End),
                order: Order.Descending("id"),
                take: 1
            );

            return builds[0];
        }

        public Task<IReadOnlyList<Build>> ListAsync(long projectId, int take = 1000)
        {
            var range = ScopedId.GetRange(projectId);

            return db.Builds.QueryAsync(
                expression  : Expression.Between("id", range.Start, range.End),
                order       : Order.Descending("id"),
                take        : take
            );
        }

        public async Task<Amazon.CodeBuild.Build> CheckStatusAsync(IBuild build)
        {
            var result = await builder.BatchGetBuildsAsync(new BatchGetBuildsRequest(build.ResourceId));

            var b = result.Builds[0];
          
            return b;
        }

        public async Task<Build> StartAsync(CreateBuildRequest request)
        {
            var project = request.Project;
            var commit  = request.Commit;

            var id = await BuildId.NextAsync(db.Context, project.Id).ConfigureAwait(false);

            // code build also injects
            // CODEBUILD_BUILD_ID

            var startBuildRequest = new StartBuildRequest(project.Name) { 
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

            var externalBuild = (await builder.StartBuildAsync(startBuildRequest).ConfigureAwait(false)).Build;
           
            var build = new Build(
                id          : id,
                initiatorId : request.InitiatorId,
                commitId    : commit.Id,
                resource    : ManagedResource.Build(Locations.Aws_US_East_1, externalBuild.Id)
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

    internal static class BuildId
    {
        public static async Task<long> NextAsync(
            IDbContext context,
            long projectId)
        {
            using (var connection = context.GetConnection())
            {
                var currentCommitCount = await connection.ExecuteScalarAsync<int>(
                  @"SELECT `buildCount` FROM `BuildProjects` WHERE id = @id FOR UPDATE;
                      UPDATE `BuildProjects`
                      SET `buildCount` = `buildCount` + 1
                      WHERE id = @id", new { id = projectId }).ConfigureAwait(false);

                return ScopedId.Create(projectId, currentCommitCount + 1);
            }
        }
    }
}


// arn:aws:codebuild:region-ID:account-ID:build