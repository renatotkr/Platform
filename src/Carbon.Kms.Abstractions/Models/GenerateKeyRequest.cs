using System;
using System.Collections.Generic;

using Carbon.Data.Protection;

namespace Carbon.Kms
{
    public class GenerateKeyRequest
    {
        public GenerateKeyRequest() { }

        public GenerateKeyRequest(string name, IEnumerable<KeyValuePair<string, string>> aad)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Aad = aad;
        }

        public string Name { get; set; }

        public KeyType Type { get; set; } = KeyType.Secret;

        public int Size { get; set; } = 256;

        public IEnumerable<KeyValuePair<string, string>> Aad { get; set; }
    }
}