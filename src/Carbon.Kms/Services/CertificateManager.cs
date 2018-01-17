using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Data.Expressions;
using Carbon.Data.Protection;

namespace Carbon.Kms.Services
{
    public class CertificateManager : ICertificateManager
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
                throw new CertificateNotFoundException(id);
            }

            return certificate;
        }
        
        public async Task<ICertificate> FindAsync(Hash fingerprint)
        {
            if (fingerprint.Type != HashType.SHA256)
            {
                throw new ArgumentException($"Must be SHA256. Was {fingerprint.Type}", "fingerprint");
            }
            
            return await db.Certificates.QueryFirstOrDefaultAsync(
                Expression.Eq("fingerprint", fingerprint.Data)
            );
        }

        public async Task<ICertificate> CreateAsync(CreateCertificateRequest request)
        {
            Ensure.NotNull(request, nameof(request));

            var cert = new X509Certificate2(request.Data);

            var certificateId = await db.Certificates.Sequence.NextAsync();

            if (request.ParentId != null)
            {
                // Ensure the parent exist...
                var parent = await GetAsync(request.ParentId.Value);
            }

            var certificate = new CertificateInfo(
                id                  : certificateId,
                ownerId             : request.OwnerId,
                data                : request.Data,
                parentId            : request.ParentId,
                encryptedPrivateKey : request.EncryptedPrivateKey,
                expires             : cert.NotAfter.ToUniversalTime()
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
            Ensure.NotNull(certificate, nameof(certificate));

            await db.Certificates.PatchAsync(certificate.Id, new[] {
                Change.Replace("deleted", Expression.Func("NOW"))
            }, condition: Expression.IsNull("deleted"));
        }

        public async Task RevokeAsync(ICertificate certificate)
        {
            Ensure.NotNull(certificate, nameof(certificate));

            await db.Certificates.PatchAsync(certificate.Id, new[] {
                Change.Replace("revoked", Expression.Func("NOW"))
            }, condition: Expression.IsNull("revoked"));
        }
    }
}