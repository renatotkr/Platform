namespace Carbon.Rds
{
    public class CreateDatabaseUserRequest
    {
        public long DatabaseId { get; set; }

        public long UserId { get; set; }

        public string Name { get; set; }
    }
}