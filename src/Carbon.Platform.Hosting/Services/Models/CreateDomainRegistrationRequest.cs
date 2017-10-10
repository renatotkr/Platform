﻿using System;

namespace Carbon.Platform.Hosting
{
    public struct CreateDomainRegistrationRequest
    {
        public CreateDomainRegistrationRequest(
            long domainId,
            long ownerId,
            long registrarId,
            DateTime expires)
        {
            DomainId    = domainId;
            OwnerId     = ownerId;
            RegistrarId = registrarId;
            Expires     = expires;
        }

        public long DomainId { get; }

        public long OwnerId { get; }

        public long RegistrarId { get; }

        public DateTime Expires { get; }

        // FromWhois?
    }
}