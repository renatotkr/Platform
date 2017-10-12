using System;
using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Data.Expressions;

namespace Carbon.Kms
{
    public partial class CertificateManager : ICertificateManager
    {
        private readonly KmsDb db;

        public CertificateManager(KmsDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<ICertificate> GetAsync(long id)
        {
            var certificate = await db.Certificates.FindAsync(id);

            if (certificate == null || certificate.Deleted != null)
            {
                throw new Exception($"certificate#{id} not found");
            }

            return certificate;
        }

        public async Task<ICertificate> CreateAsync(CreateCertificateRequest request)
        {
            #region Preconditions

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            #endregion

            var certificateId = await db.Certificates.Sequence.NextAsync();

            var certificate = new CertificateInfo(
                id       : certificateId,
                issuerId : request.IssuerId,
                ownerId  : request.OwnerId,
                data     : request.Data,
                expires  : request.Expires
            );

            var subjects = new CertificateSubject[request.Subjects.Length];

            for (var i = 0; i < subjects.Length; i++)
            {
                subjects[i] = new CertificateSubject(
                    certificateId : certificate.Id, 
                    path          : request.Subjects[i],
                    flags         : i == 0 ? CertificateSubjectFlags.Primary : CertificateSubjectFlags.None
                );
            }
            
            // TODO: Execute inside of a transaction

            await db.Certificates.InsertAsync(certificate);

            await db.CertificateSubjects.InsertAsync(subjects);

            return certificate;
        }
      
        public async Task DeleteAsync(ICertificate certificate)
        {
            await db.Certificates.PatchAsync(certificate.Id, new[] {
                Change.Replace("deleted", Expression.Func("NOW"))
            }, condition: Expression.IsNull("deleted"));
        }
    }
}