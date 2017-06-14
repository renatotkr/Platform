using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carbon.Platform.Diagnostics
{
    public interface IExceptionService
    {
        Task<ExceptionInfo> CreateAsync(CreateExceptionRequest request);

        Task<IReadOnlyList<ExceptionInfo>> ListAsync(long environmentId);
    }
}