using Carbon.Json;

namespace Carbon.CI
{
    public class CreateRepositoryUserRequest
    {
        public CreateRepositoryUserRequest(long repositoryId, long userId, JsonObject properties)
        {
            Validate.Id(repositoryId, nameof(repositoryId));
            Validate.Id(userId, nameof(userId));

            RepositoryId = repositoryId;
            UserId       = userId;
            Properties   = properties;
        }

        public long RepositoryId { get; }

        public long UserId { get; }

        public JsonObject Properties { get; }
    }
}