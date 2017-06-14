using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data.Expressions;
using Carbon.Platform.Resources;

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

        public Task<IReadOnlyList<Project>> ListAsync(long ownerId)
        {
            return db.Projects.QueryAsync(And(Eq("ownerId", ownerId), IsNull("deleted")));
        }

        public async Task<Project> GetAsync(long id)
        {
            return await db.Projects.FindAsync(id) 
                ?? throw ResourceError.NotFound(ResourceTypes.Project, id);
        }

        public async Task<Project> GetAsync(long ownerId, string name)
        {
            return await db.Projects.QueryFirstOrDefaultAsync(
              And(Eq("ownerId", ownerId), Eq("name", name))  
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
        public CreateProjectRequest() { }

        public CreateProjectRequest(string name, long ownerId)
        {
            Name    = name;
            OwnerId = ownerId;
        }

        public string Name { get; set; }

        public long RepositoryId { get; set; }

        public long OwnerId { get; set; }

        public ManagedResource Resource { get; set; }
    }
}