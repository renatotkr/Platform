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
            return await db.Projects.QueryFirstOrDefaultAsync(
                And(Eq("ownerId", ownerId), Eq("name", name))  
            ) ?? throw ResourceError.NotFound(ResourceTypes.Project, ownerId, name);
        }
        
        public async Task<ProjectInfo> GetAsync(IRepository repository)
        {
            return await db.Projects.QueryFirstOrDefaultAsync(
                And(Eq("repositoryId", repository.Id), IsNull("deleted"))
            );
        }

        public async Task<ProjectInfo> CreateAsync(CreateProjectRequest request)
        {
            Validate.NotNull(request, nameof(request));
            
            if (await db.Projects.ExistsAsync(And(Eq("ownerId", request.OwnerId), Eq("name", request.Name))))
            {
                throw new ResourceConflictException("project(ownerId:{request.OwnerId},name:{request.Name})");
            }

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

        public async Task<bool> DeleteAsync(ProjectInfo project, ISecurityContext context)
        {
            return await db.Projects.PatchAsync(project.Id, new[] {
                Change.Replace("deleted", Func("NOW"))
            }, condition: IsNull("deleted")) > 0;
        }
    }
}