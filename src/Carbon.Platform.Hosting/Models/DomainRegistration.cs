using System;

using Carbon.Data.Annotations;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Hosting
{
    [Dataset("DomainRegistrations")]
    public class DomainRegistration : IResource
    {
        public DomainRegistration() { }

        public DomainRegistration(
            long id,
            long domainId,
            long? ownerId,
            long registrarId,
            DateTime expires,
            DomainRegistrationFlags flags = default)
        {
            Ensure.IsValidId(id);
            Ensure.IsValidId(domainId, nameof(domainId));

            Id          = id;
            DomainId    = domainId;
            OwnerId     = ownerId;
            RegistrarId = registrarId;
            Expires     = expires;
            Flags       = flags;
        }

        [Member("id"), Key] // domainId | #
        public long Id { get; }

        [Member("domainId"), Indexed]
        public long DomainId { get; }

        [Member("ownerId"), Indexed]
        public long? OwnerId { get; }

        [Member("registrarId")]
        public long RegistrarId { get; }
       
        [Member("expires")] // May be extended
        public DateTime Expires { get; }

        [Member("flags")]
        public DomainRegistrationFlags Flags { get; }

        // Properties ?

        #region Timestaps

        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        #endregion

        #region IResource

        /// <summary>
        /// A unique id provided by the register
        /// </summary>
        [Member("resourceId")]
        [StringLength(100)]
        public string ResourceId { get; }

        // EncryptedPasswordData?

        ResourceType IResource.ResourceType => ResourceTypes.DomainRegistration;

        #endregion

    }
}

// Registration Stats ---------------------
// 2017   : 330.6M