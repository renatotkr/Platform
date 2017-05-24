using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data.Protection;
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

            Apps      = new AppsClient(this);
            Hosts     = new HostsClient(this);
            Websites  = new WebsitesClient(this);
        }

        #region Apps

        public AppsClient Apps { get; }

        public class AppsClient : Dao<ProgramDetails>
        {
            public AppsClient(PlatformClient platform)
                : base("apps", platform) { }

            public Task<ProgramDetails> GetReleaseAsync(long appId, SemanticVersion version)
                => platform.GetAsync<ProgramDetails>($"/apps/{appId}@{version}");

            // Create
            // Delete
            // RegisterInstance
            // DeregisterInstance
            // CreateRelease
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
                return platform.GetAsync<HostDetails>($"/hosts/{resource.ResourceId}");
            }

            public Task<List<ProgramDetails>> GetAppsAsync(long id)
            {
                return platform.GetAsync<List<ProgramDetails>>($"/hosts/{id}/apps");
            }
        }

        #endregion

        #region Websites

        public WebsitesClient Websites { get; }

        public class WebsitesClient : Dao<WebsiteDetails>
        {
            public WebsitesClient(PlatformClient platform)
                : base("websites", platform) { }

            public Task<WebsiteDetails> GetBranchAsync(long websiteId, string name) => 
                platform.GetAsync<WebsiteDetails>($"/websites/{websiteId}@{name}");

            public Task<WebsiteDetails> GetReleaseAsync(long websiteId, SemanticVersion version) =>
                platform.GetAsync<WebsiteDetails>($"/websites/{websiteId}@{version}");
        }

        #endregion
    }
}