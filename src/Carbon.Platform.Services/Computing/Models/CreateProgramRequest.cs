using System.ComponentModel.DataAnnotations;

namespace Carbon.Platform.Computing
{
    public class CreateProgramRequest
    {
        public CreateProgramRequest() { }

        public CreateProgramRequest(string name, long ownerId)
        {
            #region Preconditions

            Validate.Id(ownerId, nameof(ownerId));

            #endregion

            Name    = name;
            OwnerId = ownerId;
        }

        [Required, StringLength(63)]
        public string Name { get; set; }

        [StringLength(63)]
        public string Slug { get; set; }
        
        [Range(1, 2_199_023_255_552)]
        public long OwnerId { get; set; }
    }
}