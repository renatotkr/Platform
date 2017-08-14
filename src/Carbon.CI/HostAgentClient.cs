using System;
using System.Net.Http;
using System.Threading.Tasks;

using Carbon.Platform.Computing;
using Carbon.Security.Tokens;

namespace Carbon.CI
{
    public class HostAgentClient
    {
        private readonly HttpClient http = new HttpClient {
            DefaultRequestHeaders = {
                { "User-Agent", "Carbon/1.0" },
                { "Accept",     "application/json" }
            },
            Timeout = TimeSpan.FromSeconds(30)
        };

        private int port;
        private readonly ISecurityToken credential;

        public HostAgentClient(ISecurityToken credential, int port = 8000)
        {
            // subject = provider:user/1
            // issuer  = https://provider/

            this.credential = credential ?? throw new ArgumentNullException(nameof(credential));
            this.port       = port;
        }

        public async Task<string> DebugToken(IHost host)
        {
            var path = $"/token/debug";

            return await SendAsync(host, path);
        }

        public async Task<DeployResult> DeployAsync(IProgram program, IHost host)
        {
            var path = $"/programs/{program.Id}@{program.Version}/deploy";

            var result = await SendAsync(host, path);

            return new DeployResult(true, result);
        }

        public async Task<DeployResult> ActivateAsync(IProgram program, IHost host)
        {
            var path = $"/programs/{program.Id}@{program.Version}/activate";

            await SendAsync(host, path);

            return new DeployResult();
        }

        public async Task<bool> RestartAsync(IProgram program, IHost host)
        {
            #region Preconditions

            if (program == null)
                throw new ArgumentNullException(nameof(program));

            if (host == null)
                throw new ArgumentNullException(nameof(host));

            #endregion

            var path = $"/programs/{program.Id}@{program.Version}/restart";

            await SendAsync(host, path);

            return true;
        }

        #region Internal

        private async Task<string> SendAsync(IHost host, string path)
        {
            if (path.StartsWith("/"))
            {
                path = path.Trim('/');
            }

            var url = $"http://{host.Address}:{port}/{path}";

            var request = new HttpRequestMessage(HttpMethod.Post, url);

            request.Headers.Host = host.Id.ToString() + ".carbon.host";

            request.Headers.Add("Authorization", "Bearer " + credential.Value);

            using (var response = await http.SendAsync(request))
            {
                var text = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("ERROR:" + url + "|" + Environment.NewLine + text);
                }

                return text;
            }
        }

        #endregion
    }    
}
