using System;
using System.IO;
using System.Threading.Tasks;

using Carbon.Data.Expressions;
using Carbon.Json;
using Carbon.Packaging;
using Carbon.Platform.Computing;
using Carbon.Platform.Environments;
using Carbon.Storage;
using Carbon.Versioning;

namespace Carbon.Platform
{
    public class ProgramClient
    {
        private readonly ApiBase api;

        internal ProgramClient(ApiBase api)
        {
            this.api = api ?? throw new ArgumentNullException(nameof(api));
        }

        public Task<ProgramDetails[]> ListAsync(Expression filter = null)
        {
            return api.GetListAsync<ProgramDetails>($"/programs" + filter?.ToQueryString());
        }

        public Task<ProgramDetails[]> ListAsync(IEnvironment environment)
        {
            return api.GetListAsync<ProgramDetails>($"/environments/{environment.Id}/programs");
        }

        public Task<ProgramDetails[]> ListAsync(IHost host)
        {
            return api.GetListAsync<ProgramDetails>($"/hosts/{host.Id}/programs");
        }

        public Task<ProgramDetails> GetAsync(long id)
        {
            return api.GetAsync<ProgramDetails>($"/programs/{id}");
        }

        public Task<ProgramDetails> GetAsync(long id, SemanticVersion version)
        {
            return api.GetAsync<ProgramDetails>($"/programs/{id}@{version}");
        }

        public Task<ProgramDetails> CreateAsync(ProgramDetails program)
        {
            return api.PostAsync<ProgramDetails>(
                path : $"/programs",
                data : program
            );
        }

        public Task<ProgramDetails> UpdateAsync(ProgramDetails program)
        {
            return api.PatchAsync<ProgramDetails>(
                path : $"/programs/" + program.Id,
                data : program
            );
        }

        public async Task<IPackage> DownloadAsync(long id, SemanticVersion version)
        {
            var stream = await api.DownloadAsync($"/programs/{id}@{version}/package.zip");

            // All zip packages will be rooted...

            return ZipPackage.FromStream(stream, stripFirstLevel: false);
        }

        public async Task<ProgramDetails> PublishAsync(ProgramDetails program, Package package)
        {
            #region Preconditions

            if (program == null)
                throw new ArgumentNullException(nameof(program));

            if (package == null)
                throw new ArgumentNullException(nameof(package));
            
            #endregion

            using (var stream = new MemoryStream())
            {
                await package.ToZipStreamAsync(stream, leaveStreamOpen: true);

                stream.Position = 0;

                return await api.PutStreamAsync<ProgramDetails>(
                    path        : $"/programs/{program.Id}@{program.Version}",
                    contentType : "application/zip",
                    stream      : stream,
                    properties  : JsonObject.FromObject(program)
                );
            }
        }
    }
}