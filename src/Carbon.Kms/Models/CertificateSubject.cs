using System;

using Carbon.Data.Annotations;

namespace Carbon.Kms
{
    [Dataset("CertificateSubjects")]
    public class CertificateSubject
    {
        public CertificateSubject() { }

        public CertificateSubject(
            long certificateId,
            string name,
            DateTime? verified = null,
            long? domainId = null,
            CertificateSubjectFlags flags = default)
        {
            #region Preconditions

            if (certificateId <= 0)
                throw new ArgumentException("Must be > 0", nameof(certificateId));

            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Required", nameof(name));

            #endregion

            CertificateId = certificateId;
            Name          = name;
            DomainId      = domainId;
            Flags         = flags;
        }

        [Member("certificateId"), Key]
        public long CertificateId { get; }

        [Member("name"), Key]
        [Ascii, StringLength(253)]
        public string Name { get; }

        [Member("domainId"), Indexed]
        public long? DomainId { get; }

        [Member("flags")]
        public CertificateSubjectFlags Flags { get; }
    }

    public enum CertificateSubjectFlags
    {
        None    = 0,
        Primary = 1 << 0
    }
}

// web.com            | Domain
// charlotte@web.com  | User
// ip                 | 192.168.1.1

// Examples: 
// C=US, ST=California, L=San Francisco, O=Wikimedia Foundation, Inc., CN=*.wikipedia.org
// CN=web.com
// C=US, ST=Maryland, L=Pasadena, O=Brent Baccala, OU=FreeSoft, CN=www.freesoft.org/emailAddress=baccala @freesoft.org

// Alternative DNS Names
// DNS:magpie, DNS:magpie.example.com, DNS:puppet, DNS:puppet.example.com

// Subject Fields
// - CN : Common name
// - DN : Distingushed name
// - O  : Organization
// - C  : Country
// - ST : State
// - L  : Locality

// Alternate subject names