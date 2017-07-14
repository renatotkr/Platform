using System;

using Carbon.Data;

namespace Carbon.Rds
{
    public class RdsDb
    {
        public RdsDb(IDbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));

            // context.Types.TryAdd(new JsonObjectHandler());
            // context.Types.TryAdd(new StringArrayHandler());

            Databases          = new Dataset<DatabaseInfo,         long>(context);
            DatabaseBackups    = new Dataset<DatabaseBackup,       long>(context);
            DatabaseClusters   = new Dataset<DatabaseCluster,      long>(context);
            DatabaseEndpoints  = new Dataset<DatabaseEndpoint,     long>(context);
            DatabaseInstances  = new Dataset<DatabaseInstance,     long>(context);
            DatabaseMigrations = new Dataset<DatabaseMigration,    long>(context);
            DatabaseSchemas    = new Dataset<DatabaseSchema,       long>(context);
            DatabaseGrants     = new Dataset<DatabaseGrant,        long>(context);
            DatabaseUsers      = new Dataset<DatabaseUser, (long, long)>(context);
        }

        public IDbContext Context { get; }
        
        public Dataset<DatabaseInfo,      long> Databases          { get; }
        public Dataset<DatabaseBackup,    long> DatabaseBackups    { get;}
        public Dataset<DatabaseCluster,   long> DatabaseClusters   { get; }
        public Dataset<DatabaseEndpoint,  long> DatabaseEndpoints  { get; }
        public Dataset<DatabaseInstance,  long> DatabaseInstances  { get; }
        public Dataset<DatabaseMigration, long> DatabaseMigrations { get; }
        public Dataset<DatabaseSchema,    long> DatabaseSchemas    { get; }
        public Dataset<DatabaseGrant,     long> DatabaseGrants     { get; }

        public Dataset<DatabaseUser, (long databaseId, long userId)> DatabaseUsers { get; }

    }
}