using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data.Protection;
using Carbon.Platform.Computing;
using Carbon.Platform.Web;
using Carbon.Versioning;

namespace Carbon.Platform
{
    using Resources;

    public partial class PlatformClient
    {
        private readonly string host;
        private readonly string baseUri;
        private readonly Secret secret;
        
        public PlatformClient(string host, Secret secret)
        {
            this.host = host ?? throw new ArgumentNullException(nameof(host));
            this.secret = secret;

            this.baseUri = "https://" + host;

            Apps     = new AppClient(this);
            Hosts    = new HostsClient(this);
            Websites = new WebsitesClient(this);
        }

        #region Apps

        public AppClient Apps { get; }

        public class AppClient : Dao<AppDetails>
        {
            public AppClient(PlatformClient platform)
                : base("apps", platform) { }

            public Task<AppDetails> GetReleaseAsync(long appId, SemanticVersion version)
                => platform.GetAsync<AppDetails>($"/apps/{appId}@{version}");           
        }

        #endregion

        #region Hosts

        public HostsClient Hosts { get; }

        public class HostsClient : Dao<HostDetails>
        {
            public HostsClient(PlatformClient platform)
                : base("hosts", platform) { }

            // hosts/aws:i-0234123
            
            public Task<HostDetails> GetAsync(ManagedResource resource)
            {
                var provider = ResourceProvider.Get(resource.ProviderId);

                return platform.GetAsync<HostDetails>($"/hosts/{provider.Code}:{resource.ResourceId}");
            }

            public Task<List<AppDetails>> ListProgramsAsync(long id)
            {
                return platform.GetAsync<List<AppDetails>>($"/hosts/{id}/programs");
            }
        }

        #endregion

        #region Websites

        public WebsitesClient Websites { get; }

        public class WebsitesClient : Dao<WebsiteDetails>
        {
            public WebsitesClient(PlatformClient platform)
                : base("websites", platform) { }

            public Task<WebsiteDetails> GetReleaseAsync(long websiteId, SemanticVersion version) =>
                platform.GetAsync<WebsiteDetails>($"/websites/{websiteId}@{version}");
        }

        #endregion
    }
}