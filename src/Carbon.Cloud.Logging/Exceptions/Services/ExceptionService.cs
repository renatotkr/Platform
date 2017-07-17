using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Cloud.Logging;
using Carbon.Data;
using Carbon.Data.Expressions;
using Carbon.Data.Sequences;

namespace Carbon.Platform.Diagnostics
{
    using static Expression;

    public class ExceptionService : IExceptionService
    {
        private readonly LogsDb db;

        public ExceptionService(LogsDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public Task<ExceptionInfo> GetAsync(Uid id)
        {
            return db.Exceptions.FindAsync(id);
        }

        public async Task<IReadOnlyList<ExceptionInfo>> ListAsync(long environmentId)
        {
            var start = RequestId.Create(environmentId, DateTime.UtcNow.AddYears(-10), 0);
            var end   = RequestId.Create(environmentId, DateTime.UtcNow.AddYears(10), 0);
            
            var result = await db.Exceptions.QueryAsync(
                expression  : Between("id", start, end),
                order       : Order.Descending("id"),
                take        : 1000
            ).ConfigureAwait(false);

            return result;
        }

        public async Task<ExceptionInfo> CreateAsync(CreateExceptionRequest request)
        {
            #region Preconditions

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            #endregion

            // 1 per millisecond for now...

            var id = RequestId.Create(
                request.EnvironmentId,
                request.Created ?? DateTime.UtcNow, 
                0 // TODO: Random
            );

            var ex = new ExceptionInfo {
                Id         = id,
                ProgramId  = request.ProgramId,
                Context    = request.Context,
                Properties = request.Properties,
                HostId     = request.HostId,
                Message    = request.Message,
                Type       = request.Type,
                StackTrace = request.StackTrace,
                RequestId  = request.RequestId
            };

            await db.Exceptions.InsertAsync(ex);

            return ex;
        }
    }
}
