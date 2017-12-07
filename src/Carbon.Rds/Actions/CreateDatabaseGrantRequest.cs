using System;

using Carbon.Data.Sql;

namespace Carbon.Rds.Services
{
    public class CreateDatabaseGrantRequest
    {
        public CreateDatabaseGrantRequest(
            IDatabaseInfo database,
            DbObject resource, 
            string[] privileges, 
            long userId)
            : this(database.Id, resource, privileges, userId) { }

        public CreateDatabaseGrantRequest(
            long databaseId,
            DbObject resource,
            string[] privileges,
            long userId)
        {
            Validate.Id(databaseId, nameof(databaseId));
            Validate.NotNull(privileges, nameof(privileges));
            Validate.Id(userId, nameof(userId));

            #region Preconditions

            if (privileges.Length == 0)
            {
                throw new ArgumentException("Must not be empty", nameof(privileges));
            }
            
            foreach (var privilege in privileges)
            {
                if (privilege == null || privilege.Length == 0 || privilege.Length > 63)
                {
                    throw new ArgumentException("Invalid", nameof(privileges));
                }
            }

            #endregion

            DatabaseId  = databaseId;
            UserId      = userId;
            Resource    = resource;
            Privileges  = privileges;
        }

        public long DatabaseId { get; }

        public string[] Privileges { get; }

        public DbObject Resource { get; }

        public long UserId { get; }
    }
}

// GRANT {privileges} on {resource} to {user}