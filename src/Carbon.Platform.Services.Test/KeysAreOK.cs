using System;
using System.Data.Common;
using System.Reflection;
using System.Threading.Tasks;

using Carbon.CI;
using Carbon.Data;
using Carbon.Data.Sql;
using Carbon.Data.Sql.Adapters;
using Carbon.Kms;
using Carbon.Platform.Storage;
using Carbon.Platform.Web;
using Carbon.Rds;

using Xunit;

namespace Carbon.Platform.Services.Test
{
    public class KeyLengthsAreUnderXBytes
    {
        private static readonly MySqlDataContext dbContext = new MySqlDataContext();

        [Fact]
        public void KmsDbIsOk()
        {
            var database = new KmsDb(dbContext);

            KeysAndIndexesAreUnder767Bytes(database);
        }

        [Fact]
        public void RdsDbIsOk()
        {
            var database = new RdsDb(dbContext);

            KeysAndIndexesAreUnder767Bytes(database);
        }


        [Fact]
        public void CiadDbKeysAndIndexesAreUnder767Bytes()
        {
            var database = new CiadDb(dbContext);

            KeysAndIndexesAreUnder767Bytes(database);
        }

        [Fact]
        public void PlatformDbKeysAndIndexesAreUnder767Bytes()
        {
            var database = new PlatformDb(dbContext);

            KeysAndIndexesAreUnder767Bytes(database);
        }

        [Fact]
        public void RepositoryDbKeysAndIndexesAreUnder767Bytes()
        {
            var database = new RepositoryDb(dbContext);

            KeysAndIndexesAreUnder767Bytes(database);
        }

        [Fact]
        public void WebDbKeysAndIndexesAreUnder767Bytes()
        {
            var database = new WebDb(dbContext);

            KeysAndIndexesAreUnder767Bytes(database);
        }

        private void KeysAndIndexesAreUnder767Bytes(object database)
        {           

            foreach (var property in database.GetType().GetProperties())
            {
                var type = property.PropertyType;

                if (!type.GetTypeInfo().IsGenericType) continue;

                type = type.GetGenericArguments()[0];

                var dataset = DatasetInfo.Get(type);

               
                foreach (var index in dataset.Indexes)
                {
                    var indexLength = 0;

                    foreach (var member in index.Members)
                    {
                        var m = dataset[member.Name];

                        indexLength += GetLength(m, dataset);
                    }

                    if (indexLength > 767)
                    {
                        throw new Exception($"table index '{index.Name}' on '{dataset.Name}' exceeds 767 bytes");
                    }
                }
              

                int primaryKeyLength = 0;

                foreach (var key in dataset.PrimaryKey)
                {
                    primaryKeyLength += GetLength(key, dataset);
                }

                if (primaryKeyLength > 767)
                {
                    throw new Exception($"primary key for '{dataset.Name}' exceeds 767 bytes. Was {primaryKeyLength} bytes.");
                }
            }
        }

        private int GetLength(IMember member, DatasetInfo dataset)
        {
            if (member.Size != null)
            {
                // strings characters may take up-to 4 bytes each
                if (member.Type == typeof(string))
                {
                    return member.Size.Value * 4;
                }

                return member.Size.Value;
            }
            else if (member.Type == typeof(int))
            {
                return 4;
            }
            else if (member.Type == typeof(long))
            {
                return 8;
            }

            throw new Exception($"unknown type '{member.Type.Name}' for {dataset.Name}/{member.Name}");
        }


        /*
        private string GetCreateTableCommand<T>()
        {

            var command = CreateTableCommand.For(typeof(T), context);

            return command.CommandText;
        }
        */
    }

    internal class MySqlDataContext : IDbContext
    {
        public ISqlAdapter SqlAdapter => MySqlAdapter.Default;

        public DbTypeMap Types => new DbTypeMap(MySqlAdapter.Default);

        public Task<DbConnection> GetConnectionAsync() => null;
    }
}
