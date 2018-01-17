using System;
using System.Threading.Tasks;

namespace Carbon.Platform.Computing
{
    public class HostProgramService : IHostProgramService
    {
        private readonly PlatformDb db;

        public HostProgramService(PlatformDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }
        
        public async Task<HostProgram> CreateAsync(CreateHostProgramRequest request)
        {            
            Ensure.Object(request, nameof(request)); // Validate the request
            
            var hostProgram = new HostProgram(
                hostId         : request.HostId,
                programId      : request.Program.Id,
                programName    : request.Program.Name,
                programVersion : request.Program.Version,
                addresses      : request.Program.Addresses
            );

            await db.HostPrograms.InsertAsync(hostProgram);

            return hostProgram;
        }
    }
}