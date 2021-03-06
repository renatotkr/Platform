﻿using Carbon.Platform.Environments;

namespace Carbon.Platform.Hosting
{
    public class BindDomainRequest
    {
        public BindDomainRequest(
            long domainId, 
            IEnvironment environment,
            long? originId = null,
            long? certificateId = null)
        {
            Ensure.IsValidId(domainId,         nameof(domainId));
            Ensure.NotNull(environment, nameof(environment));

            DomainId      = domainId;
            Environment   = environment;
            OriginId      = originId;
            CertificateId = certificateId;
        }
        
        public long DomainId { get; }
        
        public IEnvironment Environment { get; } 
        
        public long? OriginId { get; }

        public long? CertificateId { get; }
    }
}