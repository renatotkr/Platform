using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Data.Expressions;
using Carbon.Json;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    using static Expression;

    public class ProgramService : IProgramService
    {
        private readonly PlatformDb db;

        public ProgramService(PlatformDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<ProgramInfo> GetAsync(long id)
        {
            return await db.Programs.FindAsync(id).ConfigureAwait(false)
                ?? throw ResourceError.NotFound(ResourceTypes.Program, id);
        }

        public Task<ProgramInfo> FindAsync(string slug)
        {
            if (long.TryParse(slug, out var id))
            {
                return db.Programs.FindAsync(id);
            }
            else
            {
                return db.Programs.QueryFirstOrDefaultAsync(Eq("slug", slug));
            }
        }

        public Task<IReadOnlyList<ProgramInfo>> ListAsync(long ownerId)
        {
            return db.Programs.QueryAsync(
                And(Eq("ownerId", ownerId), IsNull("deleted")),
                Order.Ascending("name")
             );
        }

        public async Task<ProgramInfo> CreateAsync(CreateProgramRequest request)
        {
            #region Preconditions

            Validate.Object(request, nameof(request));

            if (request.Type != ProgramType.Site && ProgramName.Validate(request.Name) == false)
                throw new ArgumentException($"Not a valid program name '{request.Name}", nameof(request.Name));

            #endregion

            var program = new ProgramInfo(
                id         : await db.Programs.Sequence.NextAsync(),
                type       : request.Type,
                name       : request.Name,
                slug       : request.Slug,
                version    : request.Version,
                properties : new JsonObject(),
                runtime    : request.Runtime,
                addresses  : request.Addresses,
                ownerId    : request.OwnerId,
                parentId   : request.ParentId
            );
            
            await db.Programs.InsertAsync(program).ConfigureAwait(false);
            
            return program;
        }
    }
}

// Programs can be Apps, Sites, Services, or Tasks
