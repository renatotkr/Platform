using System;

namespace Carbon.Platform
{
    internal static class ZoneHelper
    {
        // Our identity model supports 255 zones

        // aws current has 5 zones 
        // gcp current has 6 zones (the F zone in Central US)

        private static readonly char[] letters = {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M',
            'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
        };

        public static byte GetNumber(char zone)
        {
            if (!char.IsUpper(zone))
            {
                zone = char.ToUpper(zone);
            }

            var index = Array.IndexOf(letters, zone);

            if (index == -1) throw new Exception("not found");

            return (byte)(index + 1);
        }
    }
}
