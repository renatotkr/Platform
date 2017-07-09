using System;

using Carbon.Data.Sequences;

namespace Carbon
{
    public static class Base62
    {
        private const int radix = 62;
        private const string symbols = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
       
        public static ulong Decode(string str)
        {
            ulong result = 0;
            ulong place = 1;
            int length = str.Length;

            for (int i = 0; i < length; ++i)
            {
                result += (ulong)symbols.IndexOf(str[length - 1 - i]) * place;
                place *= radix;
            }

            return result;
        }
  
        public static string Encode(Uid num)
        {
            return Encode(num.Upper).PadLeft(11, '0') + Encode(num.Lower).PadLeft(11, '0');
        }
     
        public static string Encode(ulong val)
        {
            // char* buffer = stackalloc char[32];

            var buffer = new char[32];

            int i = 0;

            var q = val;

            do
            {
                buffer[i++] = symbols[(int)(q % radix)];
                q /= radix;
            }
            while (q > 0);

            Array.Reverse(buffer, 0, i);

            return new string(buffer, 0, i);
        }
    }
}