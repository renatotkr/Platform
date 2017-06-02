using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Data.Protection;
using Carbon.Json;
using Carbon.Platform.Sequences;
using Carbon.Time;

using Dapper;

namespace Carbon.Kms
{
    public class DekManager : IDekManager
    {
        private readonly long kekId;
        private readonly KmsDb db;
        private readonly IClock clock;
        private readonly IDataProtector masterKey;

        public DekManager(
            long kekId,
            IClock clock,
            KmsDb db, 
            IDataProtector protector)
        {
            #region Preconditions

            if (kekId <= 0)
                throw new ArgumentException("Invalid", nameof(kekId));

            #endregion

            this.kekId     = kekId;
            this.clock     = clock     ?? throw new ArgumentNullException(nameof(clock));
            this.db        = db        ?? throw new ArgumentNullException(nameof(db));
            this.masterKey = protector ?? throw new ArgumentNullException(nameof(protector));
        }

        public async Task<IKeyInfo> CreateAsync(
            IEnumerable<KeyValuePair<string, string>> context)
        {
            var aes = Aes.Create();
            
            // keying material.

            aes.KeySize = 256;
            aes.GenerateKey();

            var ciphertext = await masterKey.EncryptAsync(aes.Key, context).ConfigureAwait(false);

            var dek = new DekInfo(
                id         : await DekId.NextAsync(db.Context, kekId).ConfigureAwait(false),
                version    : 1,
                kekId      : kekId,
                ciphertext : ciphertext,
                context    : ToJson(context)
             );

            await db.Deks.InsertAsync(dek).ConfigureAwait(false);

            return dek;
        }

        // RotateAsync...

        public async Task DeleteAsync(long id)
        {
            // Clear out the ciphertext when deleting...

            using (var connection = db.Context.GetConnection())
            {
                await connection.ExecuteAsync(
                    @"UPDATE `Deks`
                      SET `ciphertext` = NULL,
                          `deleted` = NOW()
                      WHERE `id` = @id", new { id = id }
                ).ConfigureAwait(false);
            }
        }

        /*
        public async Task GrantAccess(string principal, DataEncryptionKeyInfo dek)
        {
            var result = await kmsService.CreateGrantAsync(principal, null, new[] {
                new KeyValuePair<string, string>("dekId", dek.Id.ToString())
            });

            var grant = new DataEncryptionKeyGrant
            {

                Id = db.DataEncryptionKeyGrants.Sequence.Next(),
                DekId = 1,
                Name = "",
                Principal = principal
            };

            await db.Grants.InsertAsync(grant);
        }
        */

        #region Helpers

        private static IEnumerable<KeyValuePair<string, string>> ToKeyValuePairs(JsonObject json)
        {
            foreach (var property in json)
            {
                yield return new KeyValuePair<string, string>(property.Key, property.Value.ToString());
            }
        }

        private static JsonObject ToJson(IEnumerable<KeyValuePair<string, string>> pairs)
        {
            if (pairs == null) return null;

            var json = new JsonObject();

            foreach (var property in pairs)
            {
                json.Add(property.Key, property.Value);
            }

            return json;
        }

        #endregion
    }

    public class DekId
    {
        static readonly string sql = SqlHelper.GetCurrentValueAndIncrement<KeyInfo>("dekCount");

        public static async Task<long> NextAsync(IDbContext context, long keyId)
        {
            using (var connection = context.GetConnection())
            {
                var currentCount = await connection.ExecuteScalarAsync<int>(sql,
                    new { id = keyId }).ConfigureAwait(false);

                return ScopedId.Create(keyId, currentCount + 1);
            }
        }
    }

    internal static class SqlHelper
    {
        public static string GetCurrentValueAndIncrement<T>(string columnName)
        {
            var dataset = DatasetInfo.Get<T>();

            var column = dataset[columnName];

            return 
                $@"SELECT `{column.Name}` FROM `{dataset.Name}` WHERE id = @id FOR UPDATE;
                   UPDATE `{dataset.Name}`
                   SET `{column.Name}` = `{columnName}` + 1
                   WHERE id = @id";
        }
    }
}