using System;

using Carbon.Data;
using Carbon.Data.Sequences;

namespace Carbon.Kms
{
    public class KmsDb
    {
        public KmsDb(IDbContext context)
        {
            // context.Types.TryAdd(new JsonObjectHandler());
            context.Types.TryAdd(new UidHandler());
          
            Context = context ?? throw new ArgumentNullException(nameof(context));

            Certificates        = new Dataset<CertificateInfo, long>(context);
            CertificateSubjects = new Dataset<CertificateSubject, (long, string)>(context);

            KeyGrants           = new Dataset<KeyGrant, Uid>(context);
            Keys                = new Dataset<KeyInfo,  Uid>(context);
        }

        public IDbContext Context { get; }

        public Dataset<CertificateInfo, long>              Certificates        { get; }
        public Dataset<CertificateSubject, (long, string)> CertificateSubjects { get; }

        public Dataset<KeyInfo,  Uid> Keys      { get; }
        public Dataset<KeyGrant, Uid> KeyGrants { get; }
    }
}