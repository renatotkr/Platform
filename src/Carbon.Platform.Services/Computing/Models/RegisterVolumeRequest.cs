using System;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    public class RegisterVolumeRequest
    {
        public RegisterVolumeRequest() { }

        public RegisterVolumeRequest(ByteSize size, ManagedResource resource)
        {
            Size = size;
            Resource = resource;
        }

        public ByteSize Size { get; set; }
        
        public long? HostId { get; set; }

        public ManagedResource Resource { get; set; }
    }

    // TODO: Watch to see if this lands in CoreFx
    // https://github.com/dotnet/corefx/issues/14234

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