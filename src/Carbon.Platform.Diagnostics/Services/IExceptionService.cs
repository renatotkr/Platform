using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data.Sequences;

namespace Carbon.Platform.Diagnostics
{
    public interface IExceptionService
    {
        Task<ExceptionInfo> CreateAsync(CreateExceptionRequest request);

        Task<ExceptionInfo> FindAsync(BigId id);

        Task<IReadOnlyList<ExceptionInfo>> ListAsync(long environmentId);
    }
}