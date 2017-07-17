using System.IO;

namespace Carbon
{
    internal static class Serializer
    {
        public static T Deserialize<T>(byte[] data)
        {
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
