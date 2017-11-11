using System;
using System.IO;

namespace Carbon.Cloud.Logging
{
    internal static class Varint
    {
        public static void Encode(ulong value, Stream stream)
        {
            while (value >= 0x80)
            {
                stream.WriteByte((byte)(value | 0x80));

                value >>= 7;
            }

            stream.WriteByte((byte)value);
        }


        // This function is based on dotnet source code (MIT Licenced)
        // Copyright (c) .NET Foundation and Contributors
        public static ulong Read(Stream stream)
        {
            long count = 0;
            int shift = 0;
            byte b;
            do
            {
                if (shift == 10 * 7)
                {
                    throw new FormatException("cannot be represented by a ulong");
                }

                b = (byte)stream.ReadByte();

#pragma warning disable CS0675 // Bitwise-or operator used on a sign-extended operand
                count |= (b & 0x7F) << shift;
#pragma warning restore CS0675

                shift += 7;
            } while ((b & 0x80) != 0); // continue the msb is 1

            return (ulong)count;
        }
    }
}
