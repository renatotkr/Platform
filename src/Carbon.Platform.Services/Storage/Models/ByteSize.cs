using System;

namespace Carbon.Platform.Storage
{
    public struct ByteSize
    {
        public static readonly ByteSize Zero = new ByteSize(0);

        public ByteSize(long totalBytes)
        {
            #region Preconditions

            if (totalBytes < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(totalBytes), totalBytes, "Must be 0 or greater");
            }

            #endregion

            TotalBytes = totalBytes;
        }

        public long TotalBytes { get; }

        private const int _1GiB = 1_073_741_824;

        public static ByteSize GiB(long value) => new ByteSize(value * _1GiB);
    }
}

// TODO: Watch to see if this lands in CoreFx
// https://github.com/dotnet/corefx/issues/14234