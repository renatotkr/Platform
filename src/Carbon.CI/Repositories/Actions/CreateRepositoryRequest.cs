using System;
using System.ComponentModel.DataAnnotations;

using Carbon.Json;

namespace Carbon.CI
{
    public class CreateRepositoryRequest
    {
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
        public string Name { get; }

        [Range(1, 2_199_023_255_552)]
        public long OwnerId { get; }

        public byte[] EncryptedAccessToken { get; }

        public int ProviderId { get; }

        public string Origin { get; }

        public JsonObject Properties { get; }
    }
}