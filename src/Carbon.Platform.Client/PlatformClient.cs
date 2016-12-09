using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Carbon.Platform
{
    using Apps;
    using Computing;
    using Frontends;
    using Json;
    using Networking;
    using Protection;
    using Storage;
    using Versioning;

    public class PlatformClient
    {
        private readonly HttpClient httpClient = new HttpClient();

        private readonly string host;
        private readonly SecretKey secret;

        public PlatformClient(string host, SecretKey secret)
        {
            #region Preconditions

            if (host == null) throw new ArgumentNullException(nameof(host));

            #endregion

            this.host = host;
            this.secret = secret;

            Apps              = new AppsClient(this);
            Hosts             = new HostsClient(this);
            Frontends         = new FrontendsClient(this);
            NetworkInterfaces = new Dao<NetworkInterfaceInfo>("networkinterfaces", this);
            Volumes           = new Dao<VolumeInfo>("volumes", this);
        }

        #region Apps

        public AppsClient Apps { get; }

        public class AppsClient : Dao<App>
        {
            public AppsClient(PlatformClient platform)
                : base("apps", platform) { }

            public Task<AppRelease> GetReleaseAsync(long appId, SemanticVersion version)
                => platform.GetAsync<AppRelease>($"apps/{appId}/releases/{version}");

            // Create
            // Delete
            // RegisterInstance
            // DeregisterInstance
            // CreateRelease
        }


        #endregion

        #region Hosts

        public HostsClient Hosts { get; }

        public class HostsClient : Dao<Host>
        {
            public HostsClient(PlatformClient platform)
                : base("hosts", platform) { }

            public Task<Host> GetAsync(PlatformProviderId provider, string refId)
                => platform.GetAsync<Host>("hosts/" + refId);

            // Status...

            public Task<List<App>> GetAppsAsync(long id)
                => platform.GetAsync<List<App>>($"hosts/{id}/apps");

            public Task<List<VolumeInfo>> GetVolumesAsync(long id)
                => platform.GetAsync<List<VolumeInfo>>($"hosts/{id}/volumes");

            public Task<List<VolumeInfo>> GetNetworkInterfacesAsync(long id)
                => platform.GetAsync<List<VolumeInfo>>($"hosts/{id}/networkinterfaces");

            // Register
        }

        #endregion

        #region Frontends

        public FrontendsClient Frontends { get; }

        public class FrontendsClient : Dao<Frontend>
        {
            public FrontendsClient(PlatformClient platform)
                : base("frontends", platform) { }


            // This only gets the version...

            public Task<FrontendBranch> GetBranch(long frontendId, string name)
                => platform.GetAsync<FrontendBranch>($"frontends/{frontendId}/{name}");

            public Task<FrontendRelease> GetReleaseAsync(long frontendId, SemanticVersion version)
                => platform.GetAsync<FrontendRelease>($"frontends/{frontendId}/releases/{version}");
        }

        #endregion

        public Dao<NetworkInterfaceInfo> NetworkInterfaces { get; }

        public Dao<VolumeInfo> Volumes { get; }

        #region DAO Helper

        public class Dao<T>
            where T : new()
        {
            protected readonly PlatformClient platform;
            private readonly string prefix;
             
            public Dao(string prefix, PlatformClient platform)
            {
                this.prefix = prefix;
                this.platform = platform;
            }

            public Task<T> GetAsync(long id)
                => platform.GetAsync<T>(prefix + "/" + id);
            
        }

        protected async Task<T1> GetAsync<T1>(string path)
               where T1 : new()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://{host}/{path}") {
                Headers = {
                    { "Accept", "application/json" }
                }
            };

            Signer.SignRequest(secret, request);

            using (var response = await httpClient.SendAsync(request).ConfigureAwait(false))
            {
                var text = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                try
                {
                    return JsonObject.Parse(text).As<T1>();
                }
                catch
                {
                    throw new Exception("Error parsing:" + text);
                }
            }
        }
    }

    #endregion
}