using System;

namespace Carbon.Packaging
{
    using Protection;

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

            return new AesProtector(secret);
        }
    }
}
