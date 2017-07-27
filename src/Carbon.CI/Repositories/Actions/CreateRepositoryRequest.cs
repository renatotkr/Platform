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
            #region Preconditions

            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Required", nameof(name));

            if (string.IsNullOrEmpty(origin))
                throw new ArgumentException("Required", nameof(origin));

            if (ownerId <= 0)
                throw new ArgumentException("Must be > 0", nameof(ownerId));

            #endregion

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