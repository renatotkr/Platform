using System;
using System.Threading.Tasks;

using Carbon.Cloud.Logging;
using Carbon.Platform.Computing;
using Carbon.Security;
using Carbon.Storage;
using Carbon.Versioning;

namespace Carbon.CI
{
    public class ProgramReleaseManager : IProgramReleaseManager
    {
        private readonly IProgramReleaseService releaseService;
        private readonly IPackageManager packageManager;
        private readonly IEventLogger eventLog;

        public ProgramReleaseManager(
            IProgramReleaseService releaseService,
            IPackageManager packageManager,
            IEventLogger eventLog)
        {
            this.releaseService = releaseService ?? throw new ArgumentNullException(nameof(releaseService));
            this.packageManager = packageManager ?? throw new ArgumentNullException(nameof(packageManager)); 
            this.eventLog       = eventLog       ?? throw new ArgumentNullException(nameof(eventLog));
        }

        public async Task<ProgramRelease> CreateAsync(CreateProgramReleaseRequest request, ISecurityContext context)
        {
            #region Preconditions

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (request.Version == SemanticVersion.Zero)
                throw new ArgumentException("Must be be 0.0.0", nameof(request.Version));

            #endregion

            // Create the package ----------------------
            var package = await packageManager.CreateAsync(new CreatePackageRequest(
                programId      : request.Program.Id,
                programVersion : request.Version,
                stream         : request.Package.Stream,
                format         : request.Package.Format,
                sha256         : request.Package.Sha256,
                dekId          : request.Package.DekId
            ), context);

            // Create the release ----------------------
            var release = await releaseService.CreateAsync(new RegisterProgramReleaseRequest(
                program    : request.Program,
                version    : request.Version,
                commitId   : request.Commit?.Id ?? 0L,
                buildId    : request.Build?.Id,
                creatorId  : context.UserId.Value,
                properties : request.Program.Properties
            ));

            #region Logging

            await eventLog.CreateAsync(new Event(
                action   : "create",
                resource : "program#" + release.ProgramId + "@" + release.Version, 
                userId   : context.UserId
            ));

            #endregion

            return release;
        }

        public async Task<IPackage> DownloadAsync(IProgram program)
        {
            #region Preconditions

            if (program == null)
                throw new ArgumentNullException(nameof(program));

            #endregion

            var packages = await packageManager.ListAsync(program);

            if (packages.Count == 0)
            {
                throw new Exception($"program#{program.Id}@{program.Version} does not have a package");
            }
            
            return await packageManager.DownloadAsync(packages[0].Name);
        }
    }
}