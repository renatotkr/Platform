using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Data.Expressions;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    using static Expression;

    public class HostTemplateService : IHostTemplateService
    {
        private readonly PlatformDb db;

        public HostTemplateService(PlatformDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public Task<IReadOnlyList<HostTemplate>> ListAsync()
        {
            return db.HostTemplates.QueryAsync(IsNull("deleted"));
        }

        public Task<IReadOnlyList<HostTemplate>> ListAsync(long ownerId)
        {
            return db.HostTemplates.QueryAsync(
                And(Eq("ownerId", ownerId), IsNull("deleted"))
            );
        }

        public async Task<HostTemplate> GetAsync(long id)
        {
            return await db.HostTemplates.FindAsync(id) 
                ?? throw ResourceError.NotFound(ResourceTypes.Image, id);
        }

        public async Task<HostTemplate> CreateAsync(CreateHostTemplateRequest request)
        {            
            Validate.Object(request, nameof(request)); // Validate the request

            var templateId = await db.HostTemplates.Sequence.NextAsync();

            var template = new HostTemplate(
                id            : templateId,
                ownerId       : request.OwnerId,
                name          : request.Name,
                machineType   : request.MachineType,
                image         : request.Image,
                locationId    : request.LocationId,
                startupScript : request.StartupScript,
                properties    : request.Properties
            );

            await db.HostTemplates.InsertAsync(template);

            return template;
        }


        public async Task<bool> DeleteAsync(IHostTemplate template)
        {
            return await db.HostTemplates.PatchAsync(template.Id, new[] {
                Change.Replace("deleted", Func("NOW"))
            }, condition: IsNull("deleted")) > 0;
        }
    }
}