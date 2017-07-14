using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data.Expressions;
using Carbon.Platform.Sequences;
using Carbon.Platform.Storage;

namespace Carbon.Rds.Services
{
    public class DatabaseGrantService
    {
        private readonly RdsDb db;

        public DatabaseGrantService(RdsDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public Task<IReadOnlyList<DatabaseGrant>> ListAsync(IDatabaseInfo database)
        {
            #region Preconditions

            if (database == null)
                throw new ArgumentNullException(nameof(database));

            #endregion

            var range = ScopedId.GetRange(database.Id);

            return db.DatabaseGrants.QueryAsync(
                Expression.And(Expression.Between("id", range.Start, range.End), Expression.IsNotNull("deleted"))
            );
        }
      
        public async Task<DatabaseGrant> CreateAsync(CreateDatabaseGrantRequest request)
        {
            #region Preconditions

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            #endregion

            var id = await DatabaseGrantId.NextAsync(db.Context, request.DatabaseId);

            var grant = new DatabaseGrant(
                id         : id,
                databaseId : request.DatabaseId,
                userId     : request.UserId,
                resource   : request.Resource, 
                actions    : request.Actions
            );

            await  db.DatabaseGrants.InsertAsync(grant);

            return grant;

        }
        
    }

    public class CreateDatabaseGrantRequest
    {
        public long DatabaseId { get; set; }

        public long UserId { get; set; }

        public string Resource { get; set; }

        public string[] Actions { get; set; }
    }
}