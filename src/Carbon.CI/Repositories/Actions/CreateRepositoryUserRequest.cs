namespace Carbon.CI
{
    public class CreateRepositoryUserRequest
    {
        public CreateRepositoryUserRequest(
            long repositoryId,
            long userId, 
            string[] privileges, 
            string path = null)
        {
            Validate.Id(repositoryId, nameof(repositoryId));
            Validate.Id(userId, nameof(userId));

            RepositoryId = repositoryId;
            UserId       = userId;
            Privileges   = privileges;
            Path         = path;
        }

        public long RepositoryId { get; }

        public long UserId { get; }

        public string[] Privileges { get; }

        public string Path { get; }
    }
}