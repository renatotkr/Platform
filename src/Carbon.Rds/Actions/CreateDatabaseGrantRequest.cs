using System;

using Carbon.Data.Sql;

namespace Carbon.Rds.Services
{
    public class CreateDatabaseGrantRequest
    {
        public CreateDatabaseGrantRequest(IDatabaseInfo database, DbObject resource, string[] actions, long userId)
            : this(database.Id, resource, actions, userId) { }

        public CreateDatabaseGrantRequest(
            long databaseId,
            DbObject resource,
            string[] actions,
            long userId)
        {
            #region Preconditions

            if (databaseId <= 0)
                throw new ArgumentException("Must be > 0", nameof(databaseId));
            
            if (actions == null)
                throw new ArgumentNullException(nameof(actions));

            if (actions.Length == 0)
                throw new ArgumentException("Required", nameof(actions));

            if (userId <= 0)
                throw new ArgumentException("Must be > 0", nameof(userId));

            #endregion

            DatabaseId = databaseId;
            UserId     = userId;
            Resource   = resource;
            Actions    = actions;
        }

        public long DatabaseId { get; }

        public string[] Actions { get; }

        public DbObject Resource { get; }

        public long UserId { get; }
    }
}