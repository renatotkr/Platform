using System.ComponentModel.DataAnnotations;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Storage
{
    public class RegisterBucketRequest
    {
        public RegisterBucketRequest() { }

        public RegisterBucketRequest(string name, long ownerId, ManagedResource resource)
        {
            #region Validation

            Validate.NotNullOrEmpty(name, nameof(name));

            Validate.Id(ownerId, nameof(ownerId));

            #endregion

            Name = name;
            Resource = resource;
            OwnerId = ownerId;
        }

        [Required]
        [StringLength(63)]
        public string Name { get; set; }
        
        [Range(1, 2_199_023_255_552)]
        public long OwnerId { get; set; }

        public ManagedResource Resource { get; set; }
    }
}