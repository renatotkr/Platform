using System.ComponentModel.DataAnnotations;

using Carbon.Platform.Resources;

namespace Carbon.Platform.Storage
{
    public class CreateRepositoryRequest
    {
        public CreateRepositoryRequest() { }

        public CreateRepositoryRequest(
            string name,
            long ownerId,
            ManagedResource resource)
        {
            #region Preconditions

            Validate.NotNullOrEmpty(name, nameof(name));

            Validate.Id(ownerId, nameof(ownerId));

            #endregion

            Name     = name;
            OwnerId  = ownerId;
            Resource = resource;
        }

        [Required]
        public string Name { get; set; }

        [Range(1, 2_199_023_255_552)]
        public long OwnerId { get; set; }

        public byte[] EncryptedToken { get; set; }

        public ManagedResource Resource { get; set; }
    }
}