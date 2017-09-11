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
            #region Preconditions

            if (databaseId <= 0)
                throw new ArgumentException("Must be > 0", nameof(databaseId));
            
            if (privileges == null)
                throw new ArgumentNullException(nameof(privileges));

            if (privileges.Length == 0)
                throw new ArgumentException("Required", nameof(privileges));

            if (userId <= 0)
                throw new ArgumentException("Must be > 0", nameof(userId));

            #endregion

            DatabaseId = databaseId;
            UserId     = userId;
            Resource   = resource;
            Privileges    = privileges;
        }

        public long DatabaseId { get; }

        public string[] Privileges { get; }

        public DbObject Resource { get; }

        public long UserId { get; }
    }
}

// GRANT {privileges} on {resource} to {user}