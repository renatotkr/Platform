using System;
using System.Threading.Tasks;

using Carbon.Data.Expressions;
using Carbon.Platform.Resources;

namespace Carbon.CI
{
    public class ProjectService : IProjectService
    {
        private readonly CiadDb db;

        public ProjectService(CiadDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<Project> GetAsync(long id)
        {
            return await db.Projects.FindAsync(id) 
                ?? throw ResourceError.NotFound(ResourceTypes.Project, id);
        }

        public async Task<Project> GetAsync(long ownerId, string name)
        {
            return await db.Projects.QueryFirstOrDefaultAsync(
              Expression.And(Expression.Eq("ownerId", ownerId), Expression.Eq("name", name))  
            ) ?? throw ResourceError.NotFound(ResourceTypes.Project, ownerId, name);
        }

        public async Task<Project> CreateAsync(CreateProjectRequest request)
        {
            var project = new Project(
                id           : db.Projects.Sequence.Next(),
                name         : request.Name,
                repositoryId : request.RepositoryId,
                ownerId      : request.OwnerId,
                resource     : request.Resource
            );

            await db.Projects.InsertAsync(project).ConfigureAwait(false);
            
            return project;
        }

        // DeleteAsync
    }

    public class CreateProjectRequest
    {
        public string Name { get; set; }

        public long RepositoryId { get; set; }

        public long OwnerId { get; set; }

        public ManagedResource Resource { get; set; }
    }
}
