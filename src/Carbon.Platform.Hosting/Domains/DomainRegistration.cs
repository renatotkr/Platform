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
            long ownerId,
            long registrarId,
            DateTime expires,
            DomainRegistrationFlags flags = default)
        {
            #region Preconditions

            if (id <= 0)
                throw new ArgumentException("Must be > 0", nameof(id));

            if (domainId <= 0)
                throw new ArgumentException("Must be > 0", nameof(domainId));

            #endregion

            Id          = id;
            DomainId    = domainId;
            OwnerId     = ownerId;
            RegistrarId = registrarId;
            Expires     = expires;
            Flags       = flags;
        }

        [Member("id"), Key("domainRegistrationId")]
        public long Id { get; }

        [Member("domainId"), Indexed]
        public long DomainId { get; }

        [Member("ownerId")]
        public long OwnerId { get; }

        [Member("registrarId")]
        public long RegistrarId { get; }
       
        [Member("expires")]
        public DateTime Expires { get; }

        [Member("flags")]
        public DomainRegistrationFlags Flags { get; }

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

        ResourceType IResource.ResourceType => ResourceTypes.DomainRegistration;

        #endregion

    }

    public enum DomainRegistrationFlags
    {
        None = 0,
        Private = 1 << 0
    }
}

// Registration Stats ---------------------
// 2017   : 330.6M