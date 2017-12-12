using System;

using Carbon.Data.Annotations;
using Carbon.Json;

namespace Carbon.CI
{
    [Dataset("RepositoryUsers")]
    public class RepositoryUser
    {
        public RepositoryUser() { }

        public RepositoryUser(long repositoryId, long userId, JsonObject properties = null)
        {
            Validate.Id(repositoryId, nameof(repositoryId));
            Validate.Id(userId, nameof(userId));

            RepositoryId = repositoryId;
            UserId       = userId;
            Properties   = properties;
        }

        [Member("repositoyId"), Key]
        public long RepositoryId { get; }
        
        [Member("userId"), Key]
        [Indexed] // Index to lookup by user
        public long UserId { get; }
        
        // machineName.localPath ?
        [Member("properties")]
        [StringLength(1000)]
        public JsonObject Properties { get; }

        // TODO (e.g. read, write, admin)
        public string[] Roles { get; set; }

        #region Timestamps

        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [Member("deleted")]
        public DateTime? Deleted { get; }

        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        #endregion
    }
}
