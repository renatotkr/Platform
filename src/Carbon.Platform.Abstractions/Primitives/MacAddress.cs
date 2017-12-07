using Carbon.Extensions;
using System;

namespace Carbon.Net
{
    public readonly struct MacAddress
    {
        // TODO: Eliminate this array...

        private readonly byte[] data;

        public MacAddress(byte[] data)
        {
            this.data = data ?? throw new ArgumentNullException(nameof(data));
        }

        // azure | macAddress | 00-0D-3A-10-F1-29  | standard     | uppercase, dash seperated
        // aws   | macAddress | 02:81:60:cb:27:37  | non-standard | lowercased, colon seperated

        // 02:81:60:cb:27:37
        // 00-0D-3A-10-F1-29

        public static MacAddress Parse(string text)
        {
            var seperator = text[2];

            var parts = text.Split(seperator == '-' ? Seperators.Dash : Seperators.Colon);
            
            if (parts.Length != 6)
            {
                throw new Exception("Mac address must have 6 parts. Had " + parts.Length + " parts");
            }

            var buffer = new byte[6];
            
            for (var i = 0; i < 6; i++)
            {
                buffer[i] = Convert.ToByte(parts[i], 16);
            }

            return new MacAddress(buffer);
        }

        public override string ToString()
        {
            // TODO: Eliminate allocations here
            return BitConverter.ToString(data).Replace('-', ':').ToLower();
        }

        public byte[] GetAddressBytes() => data;
    }
}
