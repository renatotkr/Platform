using Carbon.Security;
using System;

namespace Carbon.Packaging
{

    public static class PackageProtector
    {


        // Key & IV are derived
        public static AesProtector Create(byte[] password, byte[] salt)
        {
            #region Preconditions

            if (password == null)
                throw new ArgumentNullException(nameof(password));

            if (salt == null)
                throw new ArgumentNullException(nameof(salt));

            #endregion


            var secret = SecretKey.Create(password, salt);

            /*
            var derivedKey = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA256,
                iterationCount      : 10000,
                numBytesRequested   : (256 / 8) + (128 / 8)
            );
            */

            return new AesProtector(secret);
        }
    }
}
