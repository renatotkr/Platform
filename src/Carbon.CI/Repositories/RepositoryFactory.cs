using System;
using System.Text;
using System.Threading.Tasks;

using Carbon.Kms;
using Carbon.VersionControl;

using GitHub;

namespace Carbon.CI
{
    public class RepositoryClientFactory : IRepositoryClientFactory
    {
        private readonly IDataDecryptor dataDecryptor;

        public RepositoryClientFactory(IDataDecryptor dataDecryptor)
        {
            this.dataDecryptor = dataDecryptor ?? throw new ArgumentNullException(nameof(dataDecryptor));
        }

        public async Task<IRepositoryClient> GetAsync(IRepository repository)
        {
            #region Preconditions

            if (repository == null)
                throw new ArgumentNullException(nameof(repository));

            if (repository.EncryptedAccessToken == null)
                throw new ArgumentException("Missing accessToken", nameof(repository));

            #endregion

            var origin = RevisionSource.Parse(repository.Origin);

            var accessTokenBytes = await dataDecryptor.DecryptAsync(repository.EncryptedAccessToken);

            var accessToken = new OAuth2Token(Encoding.UTF8.GetString(accessTokenBytes));

            var client = new RepositoryClient(
                accountName    : origin.AccountName,
                repositoryName : origin.RepositoryName,
                accessToken    : accessToken
            );

            return client;
        }
    }
}

// TODO: RepositoryProviderFactory
