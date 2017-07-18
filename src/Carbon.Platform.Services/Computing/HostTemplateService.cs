﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data.Expressions;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    public class HostTemplateService : IHostTemplateService
    {
        private readonly PlatformDb db;

        public HostTemplateService(PlatformDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public Task<IReadOnlyList<HostTemplate>> ListAsync()
        {
            return db.HostTemplates.QueryAsync(Expression.IsNull("deleted"));
        }

        public async Task<HostTemplate> GetAsync(long id)
        {
            return await db.HostTemplates.FindAsync(id).ConfigureAwait(false) 
                ?? throw ResourceError.NotFound(ResourceTypes.Image, id);
        }

        public async Task<HostTemplate> CreateAsync(CreateHostTemplateRequest request)
        {
            #region Preconditions
            
            Validate.Object(request, nameof(request));

            #endregion
            
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
    }
}