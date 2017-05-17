using System.ComponentModel.DataAnnotations;

using Carbon.Platform.VersionControl;

namespace Carbon.Platform.Web
{
    public class CreateWebsiteRequest
    {
        public CreateWebsiteRequest() { }

        public CreateWebsiteRequest(
            string name, 
            IEnvironment environment, 
            IRepository repository,
            long ownerId)
        {
            Name          = name;
            EnvironmentId = environment.Id;
            RepositoryId  = repository.Id;
            OwnerId       = ownerId;
        }

        [Required]
        public string Name { get; set; }

        [Range(1, 2_199_023_255_552)]
        public long EnvironmentId { get; set; }

        [Range(1, 2_199_023_255_552)]
        public long RepositoryId { get; set; }

        [Range(1, 2_199_023_255_552)]
        public long OwnerId { get; set; }
    }
}