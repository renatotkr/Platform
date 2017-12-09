namespace Carbon.Platform.Environments
{
    public class CreateEnvironmentUserRequest
    {
        public CreateEnvironmentUserRequest(
            long environmentId,
            long userId, 
            string[] roles)
        {
            Validate.Id(environmentId, nameof(environmentId));
            Validate.Id(userId,        nameof(userId));

            EnvironmentId = environmentId;
            UserId        = UserId;
            Roles         = roles;
        }

        public long EnvironmentId { get; }

        public long UserId { get; }

        public string[] Roles { get; }
    }
}