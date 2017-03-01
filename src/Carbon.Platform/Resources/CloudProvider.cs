using System;
using System.Text;

namespace Carbon.Platform
{
    public class CloudProvider
    {
        public static readonly CloudProvider Amazon    = new CloudProvider("AMZN", "Amazon");
        public static readonly CloudProvider Apple     = new CloudProvider("APPL", "Apple");
        public static readonly CloudProvider Google    = new CloudProvider("GOOG", "Google");
        public static readonly CloudProvider Facebook  = new CloudProvider("FB",   "Facebook");
        public static readonly CloudProvider Microsoft = new CloudProvider("MSFT", "Microsoft"); // Azure
        public static readonly CloudProvider Oracle    = new CloudProvider("ORCL", "Oracle");

        private CloudProvider(string code, string name)
        {
            #region Preconditions

            if (code == null)
                throw new ArgumentNullException(nameof(code));

            if (name == null)
                throw new ArgumentNullException(nameof(name));

            #endregion

            Code = code;
            Name = name;
        }
        
        public int Id
        {
            get
            {
                var bytes = Encoding.UTF8.GetBytes(Code.PadRight(4));

                Array.Reverse(bytes);

                return BitConverter.ToInt32(bytes, 0);
            }
        }

        public string Name { get; }

        public string Code { get; }

        public override string ToString() => Name;

        public static CloudProvider Get(int id)
        {
            var data = BitConverter.GetBytes(id);

            Array.Reverse(data);

            var c = Encoding.ASCII.GetString(data);

            return Parse(c.Trim());
        }

        public static CloudProvider Parse(string text)
        {
            #region Preconditions

            if (text == null)
                throw new ArgumentNullException(nameof(text));

            if (text.Length == 0)
                throw new ArgumentException("Must not be empty", nameof(text));

            #endregion

            switch (text.ToLower())
            {
                case "appl"      : return Apple;
                case "amzn"      : return Amazon;
                case "goog"      : return Google;
                case "fb"        : return Facebook;
                case "msft"      : return Microsoft;
                case "orcl"      : return Oracle;

                // Full names
                case "apple"     : return Apple;
                case "amazon"    : return Amazon;
                case "google"    : return Google;
                case "facebook"  : return Facebook;
                case "microsoft" : return Microsoft;
                case "oracle"    : return Oracle;

                default: throw new Exception("Unexpected provider: " + text);
            } 
        }
    }
}

// amzn:instance:i-07e6001e0415497e4
// goog:
// amzn:
// msft: