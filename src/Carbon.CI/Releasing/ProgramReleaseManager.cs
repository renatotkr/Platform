using System;
using System.Threading.Tasks;

using Carbon.Cloud.Logging;
using Carbon.Data.Protection;
using Carbon.Data.Sequences;
using Carbon.Json;
using Carbon.Kms;
using Carbon.Packaging;
using Carbon.Platform.Computing;
using Carbon.Security;
using Carbon.Storage;
using Carbon.Versioning;

namespace Carbon.CI
{
    public class ProgramReleaseManager : IProgramReleaseManager
    {
        private readonly IPackageStore packageStore;
        private readonly IProgramReleaseService releaseService;
        private readonly IDataProtectorProvider protectorProvider;
        private readonly IDataDecryptor dataDecrypter;
        private readonly IEventLogger log;

        public ProgramReleaseManager(
            IPackageStore packageStore, 
            IProgramReleaseService releaseService,
            IDataProtectorProvider protectorProvider,
            IDataDecryptor dataDecrypter,
            IEventLogger log)
        {
            this.packageStore      = packageStore      ?? throw new ArgumentNullException(nameof(packageStore));
            this.releaseService    = releaseService    ?? throw new ArgumentNullException(nameof(releaseService));
            this.protectorProvider = protectorProvider ?? throw new ArgumentNullException(nameof(ProgramReleaseManager.protectorProvider)); 
            this.log               = log               ?? throw new ArgumentNullException(nameof(log));
            this.dataDecrypter     = dataDecrypter     ?? throw new ArgumentNullException(nameof(dataDecrypter));
        }

        public async Task<ProgramRelease> CreateAsync(
            ProgramInfo program, 
            SemanticVersion version,
            IPackage package,
            ISecurityContext context,
            Uid? keyId = null)
        {
            #region Preconditions

            if (program == null)
                throw new ArgumentNullException(nameof(program));

            if (package == null)
                throw new ArgumentNullException(nameof(package));

            #endregion

            // 1/1.0.0.zip
            var key = program.Id + "/" + version.ToString() + ".zip";

            var properties = new JsonObject();
            
            byte[] encryptionKey = null;

            if (keyId != null)
            {
                var protector = await protectorProvider.GetAsync(keyId.Value.ToString());

                encryptionKey = Secret.Generate(256 / 8).Value;

                var cek = await protector.EncryptAsync(encryptionKey).ConfigureAwait(false);
                
                properties.Add("encryptionStrategy",  "sse");
                properties.Add("encryptionAlgorithm", "AES256");
                properties.Add("cek", cek);
            }

            var result = await packageStore.PutAsync(key, package, new PutPackageOptions {
                EncryptionKey = encryptionKey
            });

            if (result.IV != null)
            {
                properties.Add("iv", result.IV);
            }

            properties.Add("sha256",      result.Sha256);
            properties.Add("packageName", key);

            if (program.Properties != null)
            {
                foreach (var property in program.Properties)
                {
                    properties[property.Key] = property.Value;
                }
            }

            var release = await releaseService.CreateAsync(new CreateProgramReleaseRequest(
                program    : program,
                version    : version,
                properties : properties,
                creatorId  : context.UserId.Value
            ));

            #region Logging

            await log.CreateAsync(new Event(
                action   : "publish",
                resource : "program#" + program.Id, 
                context  : context)
            ).ConfigureAwait(false);

            #endregion

            return release;
        }

        public async Task<IPackage> DownloadAsync(ProgramRelease release)
        {
            #region Preconditions

            if (release == null)
                throw new ArgumentNullException(nameof(release));

            #endregion

            byte[] encryptionKey = null;

            if (release.Properties.TryGetValue("cek", out var cek))
            {
                var cekData = Convert.FromBase64String(cek.ToString());

                encryptionKey = await dataDecrypter.DecryptAsync(cekData);
            }

            var key = release.ProgramId + "/" + release.Version.ToString() + ".zip";

            return await packageStore.GetAsync(key, new GetPackageOptions {
                EncryptionKey = encryptionKey
            });
        }
    }
}