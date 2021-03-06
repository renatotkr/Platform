﻿using System;
using System.Data.Common;
using System.Threading.Tasks;

using Carbon.CI;
using Carbon.Data;
using Carbon.Data.Sql;
using Carbon.Data.Sql.Adapters;
using Carbon.Kms;
using Carbon.Platform.Hosting;
using Carbon.Platform.Metrics;
using Carbon.Platform.Web;
using Carbon.Rds;

using Xunit;

namespace Carbon.Platform.Services.Test
{
    public class KeyLengthsAreUnderXBytes
    {
        private static readonly MySqlDataContext dbContext = new MySqlDataContext();
        
        static KeyLengthsAreUnderXBytes()
        {
            dbContext.Types.TryAdd(new UidHandler());
        }

        [Fact]
        public void KmsDbIsOk()
        {
            var database = new KmsDb(dbContext);

            KeysAndIndexesAreUnder767Bytes(database);
        }
        
        [Fact]
        public void MetricsDbIsOk()
        {
            var database = new MetricsDb(dbContext);

            KeysAndIndexesAreUnder767Bytes(database);
        }


        [Fact]
        public void HostingsDbIsOk()
        {
            var database = new HostingDb(dbContext);

            KeysAndIndexesAreUnder767Bytes(database);
        }

        /*

        [Fact]
        public void A()
        {
            var a = GetCreateTableCommand<DomainAuthorization>();

            throw new Exception(a);
        }
        */

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

                if (!type.IsGenericType) continue;

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
                if (member.Type == typeof(string))
                {
                    return member.IsAscii
                        ? member.Size.Value      // ascii | 1 byte each
                        : member.Size.Value * 4; // utf8  | 1-4 bytes each (assume worst case)
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

        private string GetCreateTableCommand<T>()
        {
            var command = CreateTableCommand.For(typeof(T), null, dbContext);

            return command.CommandText;
        }

    }

    internal class MySqlDataContext : IDbContext
    {
        private readonly DbTypeMap types = new DbTypeMap(MySqlAdapter.Default);

        public MySqlDataContext()
        {
            new PlatformDb(this); // register the types
        }

        public SqlAdapter SqlAdapter => MySqlAdapter.Default;

        public DbTypeMap Types => types;

        public Task<DbConnection> GetConnectionAsync() => null;
    }

}
