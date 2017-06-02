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

            Keys      = new Dataset<KeyInfo,  long>(context);
            KeyGrants = new Dataset<KeyGrant, long>(context);
            Deks      = new Dataset<DekInfo, (long, int)>(context);
            Secrets   = new Dataset<SecretInfo, long>(context);
        }

        public IDbContext Context { get; }

        public Dataset<KeyInfo,        long> Keys      { get; }
        public Dataset<KeyGrant,       long> KeyGrants { get; }
        public Dataset<DekInfo, (long, int)> Deks      { get; }
        public Dataset<SecretInfo,     long> Secrets   { get; }
    }
}