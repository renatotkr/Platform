using System.ComponentModel.DataAnnotations;

namespace Carbon.Platform.Computing
{
    public class CreateProgramRequest
    {
        public CreateProgramRequest() { }

        public CreateProgramRequest(
            string name, 
            long ownerId, 
            string runtime = null,
            string[] urls = null)
        {
            #region Preconditions

            Validate.Id(ownerId, nameof(ownerId));

            #endregion

            Name    = name;
            OwnerId = ownerId;
            Runtime = runtime;
            Urls    = urls;
        }

        [Required, StringLength(63)]
        public string Name { get; set; }
        
        [StringLength(50)]
        public string Runtime { get; set; }

        public string[] Urls { get; set; }

        [StringLength(63)]
        public string Slug { get; set; }
        
        [Range(1, 2_199_023_255_552)]
        public long OwnerId { get; set; }
    }
}