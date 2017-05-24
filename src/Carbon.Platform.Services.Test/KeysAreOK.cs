using System;
using System.Data;
using System.Reflection;

using Carbon.Data;
using Carbon.Data.Sql;
using Carbon.Data.Sql.Adapters;
using Carbon.Platform.Storage;
using Carbon.Platform.Web;

using Xunit;

namespace Carbon.Platform.Services.Test
{
    public class KeyLengthsAreUnderXBytes
    {
        [Fact]
        public void PlatformDbKeysAndIndexesAreUnder767Bytes()
        {
            var database = new PlatformDb(new MySqlDataContext());

            KeysAndIndexesAreUnder767Bytes(database);
        }

        [Fact]
        public void RepositoryDbKeysAndIndexesAreUnder767Bytes()
        {
            var database = new RepositoryDb(new MySqlDataContext());

            KeysAndIndexesAreUnder767Bytes(database);
        }

        [Fact]
        public void WebDbKeysAndIndexesAreUnder767Bytes()
        {
            var database = new WebDb(new MySqlDataContext());

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

                        indexLength += GetLength(m);
                    }

                    if (indexLength > 767)
                    {
                        throw new Exception($"table index '{index.Name}' on '{dataset.Name}' exceeds 767 bytes");
                    }
                }
              

                int primaryKeyLength = 0;

                foreach (var key in dataset.PrimaryKey)
                {
                    primaryKeyLength += GetLength(key);
                }

                if (primaryKeyLength > 767)
                {
                    throw new Exception($"primary key for '{dataset.Name}' exceeds 767 bytes. Was {primaryKeyLength} bytes.");
                }
            }
        }

        private int GetLength(IMember member)
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

            throw new Exception("unknown type:" + member.Type.Name);
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

        public IDbConnection GetConnection() => null;
    }
}
