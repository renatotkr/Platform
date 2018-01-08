using System;

using Carbon.Data.Annotations;
using Carbon.Json;

namespace Carbon.Kms
{
    [Dataset("CertificateSubjects")]
    public class CertificateSubject
    {
        public CertificateSubject() { }

        public CertificateSubject(
            long certificateId,
            string path,
            JsonObject claims = null,
            CertificateSubjectFlags flags = default)
        {
            Validate.Id(certificateId, nameof(certificateId));
            Validate.NotNullOrEmpty(path, nameof(path));

            CertificateId = certificateId;
            Path          = path;
            Flags         = flags;
        }

        [Member("certificateId"), Key]
        public long CertificateId { get; }

        // ai/processor
        // ai/processor@charlotte

        [Member("path"), Key]
        [Ascii, StringLength(500)]
        public string Path { get; }

        // CN=a
        // DNS=a
        [Member("name")]
        [StringLength(1000)]
        public string Name { get; set; }

        [Member("flags")]
        public CertificateSubjectFlags Flags { get; }
    }
}