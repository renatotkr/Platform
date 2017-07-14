using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Data.Expressions;
using Carbon.Platform.Resources;
using Carbon.Security;

namespace Carbon.CI
{
    using static Expression;

    public class ProjectService : IProjectService
    {
        private readonly CiadDb db;

        public ProjectService(CiadDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public Task<IReadOnlyList<ProjectInfo>> ListAsync(long ownerId)
        {
            return db.Projects.QueryAsync(And(Eq("ownerId", ownerId), IsNull("deleted")));
        }

        public async Task<ProjectInfo> GetAsync(long id)
        {
            return await db.Projects.FindAsync(id) 
                ?? throw ResourceError.NotFound(ResourceTypes.Project, id);
        }

        public async Task<ProjectInfo> GetAsync(long ownerId, string name)
        {
            #region Preconditions

            if (name == null)
                throw new ArgumentNullException(nameof(name));

            #endregion

            return await db.Projects.QueryFirstOrDefaultAsync(
                And(Eq("ownerId", ownerId), Eq("name", name))  
            ) ?? throw ResourceError.NotFound(ResourceTypes.Project, ownerId, name);
        }

        public async Task<ProjectInfo> CreateAsync(CreateProjectRequest request)
        {
            #region Preconditions

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            #endregion

            var project = new ProjectInfo(
                id           : await db.Projects.Sequence.NextAsync(),
                name         : request.Name,
                repositoryId : request.RepositoryId,
                ownerId      : request.OwnerId,
                resource     : request.Resource
            );

            await db.Projects.InsertAsync(project);
            
            return project;
        }

        public async Task DeleteAsync(ProjectInfo project, ISecurityContext context)
        {
            #region Preconditions

            if (project == null)
                throw new ArgumentNullException(nameof(project));

            if (context == null)
                throw new ArgumentNullException(nameof(context));

            #endregion

            await db.Projects.PatchAsync(project.Id, new[] {
                Change.Replace("deleted", Func("NOW"))
            });
        }
    }
}