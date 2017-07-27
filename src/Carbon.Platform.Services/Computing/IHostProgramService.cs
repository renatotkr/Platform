using System.Threading.Tasks;

namespace Carbon.Platform.Computing
{
    public interface IHostProgramService
    {
        Task<HostProgram> CreateAsync(CreateHostProgramRequest request);
    }
}