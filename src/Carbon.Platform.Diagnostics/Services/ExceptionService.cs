using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Data.Expressions;
using Carbon.Data.Sequences;
using Carbon.Json;

namespace Carbon.Platform.Diagnostics
{
    using static Expression;

    public class ExceptionService : IExceptionService
    {
        private readonly DiagnosticsDb db;

        public ExceptionService(DiagnosticsDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<IReadOnlyList<ExceptionInfo>> ListAsync(long environmentId)
        {
            var start = BigId.Create(environmentId, DateTime.UtcNow.AddYears(-10), 0);
            var end   = BigId.Create(environmentId, DateTime.UtcNow.AddYears(10), 0);
            
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

            var id = BigId.Create(request.EnvironmentId, DateTime.UtcNow, 0);

            var ex = new ExceptionInfo {
                Id         = id,
                Context    = request.Context ?? new JsonObject(),
                Properties    = request.Details ?? new JsonObject(),
                HostId     = request.HostId,
                Message    = request.Message,
                SessionId  = request.SessionId,
                Type       = request.Type,
                StackTrace = request.StackTrace
            };

            await db.Exceptions.InsertAsync(ex);

            return ex;
        }
    }
}
