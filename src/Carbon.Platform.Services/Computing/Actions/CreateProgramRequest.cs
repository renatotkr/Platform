using System.ComponentModel.DataAnnotations;
using Carbon.Json;
using Carbon.Versioning;

namespace Carbon.Platform.Computing
{
    public class CreateProgramRequest
    {
        public CreateProgramRequest() { }

        public CreateProgramRequest(
            long ownerId,
            string name, 
            string runtime,
            string[] addresses,
            ProgramType type = ProgramType.App,
            long? repositoryId = null,
            long? parentId = null)
        {
            Ensure.Id(ownerId, nameof(ownerId));
            Ensure.NotNullOrEmpty(name, nameof(name));

            OwnerId      = ownerId;
            Name         = name;
            Addresses    = addresses;
            Runtime      = runtime;
            Type         = type;
            RepositoryId = repositoryId;
            ParentId     = parentId;
        }

        [Range(1, 2_199_023_255_552)]
        public long OwnerId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        public SemanticVersion Version { get; set; } = SemanticVersion.Zero;

        public string Runtime { get; set; }

        public string[] Addresses { get; set; }

        public ProgramType Type { get; set; }

        [StringLength(63)]
        public string Slug { get; set; }
        
        public long? ParentId { get; set; }

        public long? RepositoryId { get; set; }

        public JsonObject Properties { get; set; }
    }
}