using System;

using Carbon.Data;

namespace Carbon.Kms
{    
    public class KmsDb
    {
        public KmsDb(IDbContext context)
        {
            // context.Types.TryAdd(new JsonObjectHandler());
          
            Context = context ?? throw new ArgumentNullException(nameof(context));

            Certificates = new Dataset<CertificateInfo, long>(context);
            Vaults       = new Dataset<VaultInfo,       long>(context);
            Grants       = new Dataset<VaultGrant,      long>(context);
            Keys         = new Dataset<KeyInfo,         long>(context);
            Secrets      = new Dataset<SecretInfo,      long>(context);
        }

        public IDbContext Context { get; }

        public Dataset<CertificateInfo, long> Certificates { get; }
        public Dataset<VaultInfo,       long> Vaults       { get; }
        public Dataset<VaultGrant,      long> Grants       { get; }
        public Dataset<KeyInfo,         long> Keys         { get; }
        public Dataset<SecretInfo,      long> Secrets      { get; }
    }
}