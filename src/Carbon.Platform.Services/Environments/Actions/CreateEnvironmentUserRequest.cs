namespace Carbon.Platform.Environments
{
    public class CreateEnvironmentUserRequest
    {
        public CreateEnvironmentUserRequest(
            long environmentId,
            long userId, 
            string[] roles,
            string[] privileges)
        {
            Ensure.IsValidId(environmentId, nameof(environmentId));
            Ensure.IsValidId(userId,        nameof(userId));

            EnvironmentId = environmentId;
            UserId        = UserId;
            Roles         = roles;
            Privileges    = privileges;
        }

        public long EnvironmentId { get; }

        public long UserId { get; }

        public string[] Roles { get; }
        
        public string[] Privileges { get; }
    }
}