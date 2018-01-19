using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

using Carbon.Data.Sequences;
using Carbon.Security;

namespace Carbon.Cloud.Logging
{
    public static class ClientHash
    {
        public const int Length = 20; // sha1

        public static byte[] Compute(ISecurityContext context)
        {
            Ensure.NotNull(context, nameof(context));

            return Compute(Uid.Parse(context.ClientId), context.ClientIp, context.UserAgent);
        }

        public static byte[] Compute(IClient client)
        {
            Ensure.NotNull(client, nameof(client));

            return Compute(Uid.Parse(client.Id), client.Ip, client.UserAgent);
        }

        public static byte[] Compute(Uid id, IPAddress ip, string userAgent)
        {
            Ensure.NotNull(ip, nameof(ip));
            Ensure.NotNull(userAgent, nameof(userAgent));

            // TODO: Avoid some of these allocations....

            var idBytes        = id.Serialize();
            var addressBytes   = ip.GetAddressBytes();
            var userAgentBytes = Encoding.UTF8.GetBytes(userAgent);
            
            var size = idBytes.Length + addressBytes.Length + userAgentBytes.Length;

            using (var ms = new MemoryStream(size))
            {
                ms.Write(idBytes, 0, 16);
                ms.Write(addressBytes, 0, addressBytes.Length);
                ms.Write(userAgentBytes, 0, userAgentBytes.Length);

                using (var sha1 = SHA1.Create())
                {
                    return sha1.ComputeHash(ms.ToArray());
                }
            }
        }
    }
}
