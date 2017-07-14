using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Amazon.Helpers;

using Carbon.Data;
using Carbon.Data.Expressions;
using Carbon.Platform;
using Carbon.Platform.Resources;
using Carbon.Platform.Sequences;

using codebuild = Amazon.CodeBuild;

namespace Carbon.CI
{
    public class BuildManager : IBuildManager
    {
        private readonly codebuild::CodeBuildClient codebuild;
        private readonly CiadDb db;

        public BuildManager(codebuild::CodeBuildClient codebuild, CiadDb db)
        {
            this.db        = db        ?? throw new ArgumentNullException(nameof(db));
            this.codebuild = codebuild ?? throw new ArgumentNullException(nameof(codebuild));
        }

        public async Task<Build> GetAsync(long id)
        {
            return await db.Builds.FindAsync(id)
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
                expression : Expression.Between("id", range.Start, range.End),
                order      : Order.Descending("id"),
                take       : take
            );
        }

        public async Task<Build> SyncAsync(Build build)
        {
            if (build.Status != BuildStatus.Pending)
            {
                return build;
            }

            var result = await codebuild.BatchGetBuildsAsync(
                new codebuild::BatchGetBuildsRequest(build.ResourceId)
            );

            var external = result.Builds[0];
            
            build.Started   = external.StartTime;
            build.Completed = external.EndTime;
            build.Status    = external.GetStatus();
            build.Phase     = external.CurrentPhase?.ToLower();

            await UpdateAsync(build);

            return build;
        }

        public async Task<Build> StartAsync(StartBuildRequest request)
        {
            #region Preconditions

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            #endregion

            var project = request.Project;
            var commit  = request.Commit;

            var id = await BuildId.NextAsync(db.Context, project.Id);

            // code build also injects
            // CODEBUILD_BUILD_ID

            var startBuildRequest = new codebuild::StartBuildRequest(project.Name) { 
                EnvironmentVariablesOverride = new[] {
                    new codebuild::EnvironmentVariable("BUILD_ID", id.ToString())
                },
                SourceVersion = HexString.FromBytes(commit.Sha1)
            };

            if (request.Output != null)
            {
                startBuildRequest.ArtifactsOverride = new codebuild::ProjectArtifacts {
                    Type      = "S3",
                    Location  = request.Output.BucketName,
                    Path      = request.Output.Path,
                    Name      = request.Output.Name ?? id.ToString(),
                    Packaging = "NONE", // otherwise ZIP
                };
            }

            var codeBuildRegion = Locations.Get(ResourceProvider.Aws, codebuild.Region.Name);

            var externalBuild = await codebuild.StartBuildAsync(startBuildRequest);

            var build = new Build(
                id          : id,
                creatorId : request.InitiatorId,
                commitId    : commit.Id,
                resource    : ManagedResource.Build(codeBuildRegion, externalBuild.Build.Id)
            )
            {
                Status = BuildStatus.Pending
            };

            await db.Builds.InsertAsync(build);

            return build;
        }

        public async Task UpdateAsync(Build build)
        {
            await db.Builds.UpdateAsync(build);
        }
    }
}

// arn:aws:codebuild:region-ID:account-ID:build