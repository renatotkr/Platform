using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Data.Expressions;
using Carbon.Data.Protection;
using Carbon.Json;
using Carbon.Time;

namespace Carbon.Kms
{
    public class KeyManager : IKeyManager
    {
        private readonly long vaultId;
        private readonly KmsDb db;
        private readonly IClock clock;
        private readonly IKeyProtector vault;

        public KeyManager(
            IClock clock,
            KmsDb db, 
            long vaultId,
            IKeyProtector vault)
        {
            #region Preconditions

            if (vaultId <= 0)
                throw new ArgumentException("Invalid", nameof(vaultId));

            #endregion

            this.vaultId = vaultId;
            this.clock   = clock     ?? throw new ArgumentNullException(nameof(clock));
            this.db      = db        ?? throw new ArgumentNullException(nameof(db));
            this.vault   = vault ?? throw new ArgumentNullException(nameof(vault));
        }

        // TODO: Generate a private key...

        public async Task<IKeyInfo> PutAsync(
            byte[] plaintext,
            KeyType type,
            IEnumerable<KeyValuePair<string, string>> context)
        {
            var ciphertext = await vault.EncryptAsync(plaintext, context).ConfigureAwait(false);

            var key = new KeyInfo(
                id          : await KeyId.NextAsync(db.Context, vaultId).ConfigureAwait(false),
                name        : Guid.NewGuid().ToString().Replace("-", ""),
                kekId       : vaultId,
                vaultId     : vaultId,
                ciphertext  : ciphertext,
                activated   : DateTime.UtcNow.AddMinutes(-5),
                type        : type,
                context     : ToJson(context)
            );

            await db.Keys.InsertAsync(key).ConfigureAwait(false);

            return key;
        }

        public async Task<IKeyInfo> GenerateAsync(
            IEnumerable<KeyValuePair<string, string>> context)
        {
            var aes = Aes.Create();
            
            // keying material.

            aes.KeySize = 256;
            aes.GenerateKey();

            var ciphertext = await vault.EncryptAsync(aes.Key, context).ConfigureAwait(false);

            var key = new KeyInfo(
                id         : await KeyId.NextAsync(db.Context, vaultId).ConfigureAwait(false),
                name       : Guid.NewGuid().ToString().Replace("-", ""),
                kekId      : vaultId,
                vaultId    : vaultId,
                ciphertext : ciphertext,
                activated  : DateTime.UtcNow.AddMinutes(-5),
                type       : KeyType.Secret,
                context    : ToJson(context)
            );

            await db.Keys.InsertAsync(key).ConfigureAwait(false);

            return key;
        }

        // RotateAsync...

        public async Task DeactivateAsync(long id)
        {
            await db.Keys.PatchAsync(id, changes: new[] {
                Change.Remove("activated"),
                Change.Replace("status", KeyStatus.Deactivated)
            }).ConfigureAwait(false);
        }

        public async Task DeleteAsync(long id)
        {
            await db.Keys.PatchAsync(id, changes: new[] {
                Change.Remove("ciphertext"),
                Change.Replace("deleted", Expression.Func("NOW"))
            }).ConfigureAwait(false);
        }

        /*
        public async Task<VaultGrant> CreateGrant(string principal, IKeyInfo grant)
        {
            var result = await kmsService.CreateGrantAsync(principal, null, new[] {
                new KeyValuePair<string, string>("keyid", grant.Id.ToString())
            });

            var grant = new VaultGrant(
                id = GrantId.NextAsync(db.Context, vaultId),
                keyId: 1,
                permissions: KeyUsage.Decrypt,
                userId: 1,
                externalId: null
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
}