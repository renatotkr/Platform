using System;

namespace Carbon.Rds.Services
{
    public readonly struct CreateDatabaseUserRequest
    {
        public CreateDatabaseUserRequest(IDatabaseInfo database, long userId, string name)
            : this(database.Id, userId, name) { }

        public CreateDatabaseUserRequest(long databaseId, long userId, string name)
        {
            #region Preconditions

            if (databaseId <= 0)
                throw new ArgumentException("Must be >= 0", nameof(databaseId));

            if (userId <= 0)
                throw new ArgumentException("Must be >= 0", nameof(userId));

            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Required", nameof(name));

            #endregion

            DatabaseId = databaseId;
            UserId     = userId;
            Name       = name;
        }

        public long DatabaseId { get; }

        public long UserId { get; }

        public string Name { get; }
    }
}