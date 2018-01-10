using System;

using Carbon.Data.Annotations;

namespace Carbon.CI
{
    [Dataset("RepositoryUsers")]
    public class RepositoryUser
    {
        public RepositoryUser() { }

        public RepositoryUser(
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

        [Member("repositoryId"), Key]
        public long RepositoryId { get; }
        
        [Member("userId"), Key]
        [Indexed] // Index to lookup by user
        public long UserId { get; }
      
        // specific privileges granted to the user
        // e.g. read, write
        [Member("privileges")]
        public string[] Privileges { get; }

        // The path relative to their working directory
        // e.g. /carbonmade/lefty
        [Member("path")]
        public string Path { get; }

        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [Member("deleted")]
        public DateTime? Deleted { get; }

        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }
    }

    // A permission is a property of an object, such as a file.
    // A privilege is a property of an agent, such as a user.
}
