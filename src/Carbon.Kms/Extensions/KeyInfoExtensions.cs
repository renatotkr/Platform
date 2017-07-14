using System.Collections.Generic;

namespace Carbon.Kms
{
    public static class KeyInfoExtensions
    {
        public static IEnumerable<KeyValuePair<string, string>> GetAad(this KeyInfo keyInfo)
        {
            if (keyInfo.Aad == null) yield break;

            foreach (var property in keyInfo.Aad)
            {
                yield return new KeyValuePair<string, string>(property.Key, property.Value.ToString());
            }
        }
    }
}