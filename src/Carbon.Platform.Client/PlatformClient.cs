using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Packaging;
using Carbon.Platform.Computing;
using Carbon.Platform.Security;
using Carbon.Storage;
using Carbon.Versioning;

namespace Carbon.Platform
{
    using Resources;

    public partial class PlatformClient
    {
        private readonly string baseUri;
        private readonly Credential credentials;
        
        public PlatformClient(string baseUri, Credential credential)
        {
            if (!baseUri.StartsWith("https://"))
            {
                throw new ArgumentException("Must be https", nameof(baseUri));
            }

            this.baseUri     = baseUri.TrimEnd('/');
            this.credentials = credential;

            Programs = new ProgramClient(this);
            Hosts    = new HostsClient(this);
        }

        #region Apps

        public ProgramClient Programs { get; }

        public class ProgramClient : Dao<ProgramDetails>
        {
            public ProgramClient(PlatformClient platform)
                : base("programs", platform) { }

            public Task<ProgramDetails> GetAsync(long id, SemanticVersion version)
            {
                return platform.GetAsync<ProgramDetails>($"/programs/{id}@{version}");
            }

            public async Task<IPackage> DownloadAsync(long id, SemanticVersion version)
            {
                var packageStream = await platform.DownloadAsync(
                    $"/programs/{id}@{version}/package.zip");

                return ZipPackage.FromStream(packageStream, true);
            }
        }

        #endregion

        #region Hosts

        public HostsClient Hosts { get; }

        public class HostsClient : Dao<HostDetails>
        {
            public HostsClient(PlatformClient platform)
                : base("hosts", platform) { }

            // hosts/aws:i-0234123

            public Task<HostDetails> RegisterAsync(HostDetails host)
            {
                var provider = ResourceProvider.Get(host.Resource.ProviderId);

                return platform.PostAsync<HostDetails>(
                    $"/hosts/{provider.Code}:{host.Resource.ResourceId}", host);
            }

            public Task<HostDetails> GetAsync(ManagedResource resource)
            {
                var provider = ResourceProvider.Get(resource.ProviderId);

                return platform.GetAsync<HostDetails>($"/hosts/{provider.Code}:{resource.ResourceId}");
            }

            public Task<List<ProgramDetails>> ListProgramsAsync(long id)
            {
                return platform.GetAsync<List<ProgramDetails>>($"/hosts/{id}/programs");
            }
        }

        #endregion
    }
}