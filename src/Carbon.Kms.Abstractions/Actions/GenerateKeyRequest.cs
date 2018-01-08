using System;
using System.Collections.Generic;

using Carbon.Data.Protection;

namespace Carbon.Kms
{
    public class GenerateKeyRequest
    {
        public GenerateKeyRequest(
            long ownerId,
            string name, 
            KeyType type = KeyType.Secret, 
            int size = 256, 
            IEnumerable<KeyValuePair<string, string>> aad = null)
        {
            #region Preconditions

            if (type == default)
                throw new ArgumentException(nameof(type));
            
            if (size <= 0)
                throw new ArgumentOutOfRangeException(nameof(size), size, "Must be > 0");

            #endregion

            OwnerId = ownerId;
            Name    = name;
            Type    = type;
            Size    = size; 
            Aad     = aad;
        }

        public long OwnerId { get; }

        public string Name { get; }

        public KeyType Type { get; } 

        public int Size { get; }

        public IEnumerable<KeyValuePair<string, string>> Aad { get; }
    }
}