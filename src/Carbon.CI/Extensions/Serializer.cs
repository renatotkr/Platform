using System;
using System.IO;

namespace Carbon.Serialization
{
    internal static class Serializer
    {
        public static T Deserialize<T>(byte[] data)
        {
            #region Preconditons

            if (data == null) throw new ArgumentNullException(nameof(data));

            #endregion

            using (var ms = new MemoryStream(data))
            {
                return ProtoBuf.Serializer.Deserialize<T>(ms);
            }
        }

        public static byte[] Serialize<T>(T value)
        {
            using (var ms = new MemoryStream())
            {
                ProtoBuf.Serializer.Serialize(ms, value);

                return ms.ToArray();
            }
        }
    }
}
