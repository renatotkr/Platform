using System;
using System.ComponentModel.DataAnnotations;

using Carbon.Json;

namespace Carbon.CI
{
    public class CreateRepositoryRequest
    {
        public CreateRepositoryRequest() { }

        public CreateRepositoryRequest(
            string name,
            long ownerId,
            int providerId, 
            string origin,
            byte[] encryptedAccessToken = null,
            JsonObject properties = null)
        {
            Validate.NotNullOrEmpty(name, nameof(name));
            Validate.NotNullOrEmpty(origin, nameof(origin));
            Validate.Id(ownerId, nameof(ownerId));

            Name                 = name;
            OwnerId              = ownerId;
            ProviderId           = providerId;
            Origin               = origin;
            EncryptedAccessToken = encryptedAccessToken;
            Properties           = properties;
        }

        [Required]
        public string Name { get; set; }

        [Range(1, 2_199_023_255_552)]
        public long OwnerId { get; set; }

        public byte[] EncryptedAccessToken { get; set; }

        public int ProviderId { get; set; }

        [Required]
        public string Origin { get; set; }

        public JsonObject Properties { get; set; }
    }
}

// NOTE: keep set properties for JSON binding