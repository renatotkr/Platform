using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Carbon.Security;

namespace Carbon.Cloud.Logging
{
    public static class ClientHash
    {
        public const int Length = 20; // sha1

        public static byte[] Compute(ISecurityContext context)
        {
            return Compute(context.ClientId, context.ClientIp, context.UserAgent);
        }

        public static byte[] Compute(IClient client)
        {
            return Compute(client.Id, client.Ip, client.UserAgent);
        }

        public static byte[] Compute(Guid id, IPAddress ip, string userAgent)
        {
            #region Preconditions

            if (ip == null)
                throw new ArgumentNullException(nameof(ip));

            if (userAgent == null)
                throw new ArgumentNullException(nameof(userAgent));

            #endregion

            // TODO: Avoid some of these allocations....

            var idBytes        = id.ToByteArray();
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
