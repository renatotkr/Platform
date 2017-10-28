using System;

using Carbon.Data.Annotations;

namespace Carbon.Platform.Environments
{
    [Dataset("EnvironmentUsers")]
    public class EnvironmentUser
    {
        public EnvironmentUser() { }

        public EnvironmentUser(
            long environmentId,
            long userId, 
            string[] roles)
        {
            #region Preconditions

            if (environmentId <= 0)
                throw new ArgumentException("Must be > 0", nameof(environmentId));

            if (userId <= 0)
                throw new ArgumentException("Must be > 0", nameof(userId));

            #endregion

            EnvironmentId = environmentId;
            UserId        = userId;
            Roles         = roles ?? throw new ArgumentNullException(nameof(roles));
        }

        [Member("environmentId"), Key]
        public long EnvironmentId { get; }

        [Member("userId"), Key]
        public long UserId { get; }
        
        [Member("roles")]
        public string[] Roles { get; }

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
