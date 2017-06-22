using System;
using System.Threading.Tasks;

using Carbon.Data.Protection;
using Carbon.Json;
using Carbon.Kms;
using Carbon.Packaging;
using Carbon.Platform.Computing;
using Carbon.Serialization;
using Carbon.Storage;
using Carbon.Versioning;

namespace Carbon.CI
{
    public class ProgramReleaseManager : IProgramReleaseManager
    {
        private readonly IPackageStore packageStore;
        private readonly IProgramReleaseService releaseService;
        private readonly IDataProtector keyProtector;

        public ProgramReleaseManager(
            IPackageStore packageStore, 
            IProgramReleaseService releaseService,
            IDataProtector keyProtector)
        {
            this.packageStore   = packageStore   ?? throw new ArgumentNullException(nameof(packageStore));
            this.releaseService = releaseService ?? throw new ArgumentNullException(nameof(releaseService));
            this.keyProtector   = keyProtector   ?? throw new ArgumentNullException(nameof(keyProtector)); 
        }

        public async Task<ProgramRelease> CreateAsync(
            ProgramInfo program, 
            SemanticVersion version,
            IPackage package,
            long creatorId,
            long? keyId = null)
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
                encryptionKey = Secret.Generate(256 / 8).Value;

                var cek = keyProtector.Encrypt(keyId.Value, encryptionKey);
                
                properties.Add("encryptionStrategy",  "sse");
                properties.Add("encryptionAlgorithm", "AES256");
                properties.Add("cek", Serializer.Serialize(cek));
            }

            var result = await packageStore.PutAsync(key, package, new PutPackageOptions {
                EncryptionKey = encryptionKey
            });

            if (result.IV != null)
            {
                properties.Add("iv", result.IV);
            }

            properties.Add("sha256", result.Sha256);

            if (program.Properties != null)
            {
                foreach (var property in program.Properties)
                {
                    properties[property.Key] = property.Value;
                }
            }

            var release = new CreateProgramReleaseRequest(
                program    : program,
                version    : version,
                properties : properties,
                creatorId  : creatorId
            );

            return await releaseService.CreateAsync(release);           
        }

        public async Task<IPackage> DownloadAsync(ProgramRelease release)
        {
            #region Preconditions

            if (release == null)
                throw new ArgumentNullException(nameof(release));

            #endregion

            byte[] encryptionKey = null;

            if (release.Properties.TryGetValue("cek", out var cekData))
            {
                var cek = Serializer.Deserialize<EncryptedData>(Convert.FromBase64String(cekData.ToString()));

                encryptionKey = keyProtector.Decrypt(cek);
            }

            var key = release.ProgramId + "/" + release.Version.ToString() + ".zip";

            return await packageStore.GetAsync(key, new GetPackageOptions {
                EncryptionKey = encryptionKey
            });
        }
    }
}
