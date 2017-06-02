using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Amazon.CodeBuild;
using Amazon.Helpers;

using Carbon.Data;
using Carbon.Data.Expressions;
using Carbon.Platform;
using Carbon.Platform.Resources;
using Carbon.Platform.Sequences;

using Dapper;

namespace Carbon.CI
{
    public class BuildManager
    {
        private readonly CodeBuildClient codebuild;
        private readonly PlatformDb db;

        public BuildManager(
            CodeBuildClient codebuild, 
            PlatformDb db)
        {
            this.db        = db        ?? throw new ArgumentNullException(nameof(db));
            this.codebuild = codebuild ?? throw new ArgumentNullException(nameof(codebuild));
        }

        public async Task<Build> GetAsync(long id)
        {
            return await db.Builds.FindAsync(id).ConfigureAwait(false) 
                ?? throw ResourceError.NotFound(ResourceTypes.Build, id);
        }

        public async Task<Build> GetLatestAsync(long projectId)
        {
            var range = ScopedId.GetRange(projectId);

            var builds = await db.Builds.QueryAsync(
                expression : Expression.Between("id", range.Start, range.End),
                order      : Order.Descending("id"),
                take       : 1
            );

            return builds[0];
        }

        public Task<IReadOnlyList<Build>> ListAsync(
            long projectId, 
            int take = 1000)
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
            var result = await codebuild.BatchGetBuildsAsync(new BatchGetBuildsRequest(build.ResourceId));

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

            var externalBuild = (await codebuild.StartBuildAsync(startBuildRequest).ConfigureAwait(false)).Build;
           
            var build = new Build(
                id          : id,
                initiatorId : request.InitiatorId,
                commitId    : commit.Id,
                resource    : ManagedResource.Build(Locations.Aws_USEast1, externalBuild.Id)
            );

            await db.Builds.InsertAsync(build).ConfigureAwait(false);

            return build;
        }

        public async Task UpdateAsync(Build build)
        {
            await db.Builds.UpdateAsync(build).ConfigureAwait(false);
        }
    }
}

// arn:aws:codebuild:region-ID:account-ID:build