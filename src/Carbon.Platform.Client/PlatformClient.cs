using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Cryptography;

using Carbon.Platform.Computing;
using Carbon.Platform.Web;
using Carbon.Versioning;

namespace Carbon.Platform
{
    using System.Security.Principal;
    using Resources;

    public partial class PlatformClient
    {
        private readonly string host;
        private readonly string baseUri;
        private readonly RSA privateKey;
        private readonly string subject;
        
        public PlatformClient(string host, string subject, RSA privateKey)
        {
            this.host       = host ?? throw new ArgumentNullException(nameof(host));
            this.privateKey = privateKey;
            this.baseUri    = "https://" + host;
            this.subject    = subject;

            Programs = new ProgramClient(this);
            Hosts    = new HostsClient(this);
            Websites = new WebsitesClient(this);
        }

        #region Apps

        public ProgramClient Programs { get; }

        public class ProgramClient : Dao<ProgramDetails>
        {
            public ProgramClient(PlatformClient platform)
                : base("programs", platform) { }

            public Task<ProgramDetails> GetAsync(long id, SemanticVersion version) => 
                platform.GetAsync<ProgramDetails>($"/programs/{id}@{version}");           
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
                return platform.PostAsync<HostDetails>($"/hosts", details);
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