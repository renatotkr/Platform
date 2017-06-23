using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Packaging;
using Carbon.Platform.Computing;
using Carbon.Storage;
using Carbon.Versioning;

namespace Carbon.Platform
{
    using Resources;

    public partial class PlatformClient
    {
        private readonly string host;
        private readonly string baseUri;
        private readonly Credentials credentials;
        
        public PlatformClient(string host, Credentials credentials)
        {
            this.host        = host ?? throw new ArgumentNullException(nameof(host));
            this.baseUri     = "https://" + host;
            this.credentials = credentials;

            Programs = new ProgramClient(this);
            Hosts    = new HostsClient(this);
        }

        #region Apps

        public ProgramClient Programs { get; }

        public class ProgramClient : Dao<ProgramDetails>
        {
            public ProgramClient(PlatformClient platform)
                : base("programs", platform) { }

            public Task<ProgramDetails> GetAsync(long id, SemanticVersion version) => 
                platform.GetAsync<ProgramDetails>($"/programs/{id}@{version}");

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

            public Task<HostDetails> RegisterAsync(HostDetails details)
            {
                var provider = ResourceProvider.Get(details.Resource.Value.ProviderId);

                return platform.PostAsync<HostDetails>(
                    $"/hosts/{provider.Code}:{details.Resource.Value.ResourceId}", details);
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