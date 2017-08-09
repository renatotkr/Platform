using Carbon.Json;

namespace Carbon.CI
{
    public class CreateRepositoryUserRequest
    {
        public long RepositoryId { get; }

        public long UserId { get; set; }

        public JsonObject Properties { get; set; }
    }
}