using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data.Sequences;

namespace Carbon.Platform.Diagnostics
{
    public interface IExceptionService
    {
        Task<ExceptionInfo> CreateAsync(CreateExceptionRequest request);

        Task<ExceptionInfo> GetAsync(Uid id);

        Task<IReadOnlyList<ExceptionInfo>> ListAsync(long environmentId, int skip = 0, int take = 100);
    }
}