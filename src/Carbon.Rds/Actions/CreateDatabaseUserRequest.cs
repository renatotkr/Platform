namespace Carbon.Rds.Services
{
    public class CreateDatabaseUserRequest
    {
        public CreateDatabaseUserRequest(
            IDatabaseInfo database, 
            long userId, 
            string name)
            : this(database.Id, userId, name) { }

        public CreateDatabaseUserRequest(long databaseId, long userId, string name)
        {
            Ensure.IsValidId(databaseId, nameof(databaseId));
            Ensure.IsValidId(userId, nameof(userId));
            Ensure.NotNullOrEmpty(name, nameof(name));

            DatabaseId = databaseId;
            UserId     = userId;
            Name       = name;
        }

        public long DatabaseId { get; }

        public long UserId { get; }

        public string Name { get; }
    }
}