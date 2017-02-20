using System;

namespace Carbon.Platform
{
    public class CloudPlatformProvider
    {
        public static readonly CloudPlatformProvider Amazon    = new CloudPlatformProvider("amzn", "Amazon");
        public static readonly CloudPlatformProvider Apple     = new CloudPlatformProvider("appl", "Apple");
        public static readonly CloudPlatformProvider Google    = new CloudPlatformProvider("goog", "Google");
        public static readonly CloudPlatformProvider Microsoft = new CloudPlatformProvider("msft", "Microsoft"); // Azure
        public static readonly CloudPlatformProvider Oracle    = new CloudPlatformProvider("orcl", "Oracle");

        private CloudPlatformProvider(string code, string name)
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

        public string Name { get; }

        public string Code { get; }

        public override string ToString() => Name;

        public static CloudPlatformProvider Parse(string text)
        {
            #region Preconditions

            if (text == null)
                throw new ArgumentNullException(nameof(text));

            if (text.Length == 0)
                throw new ArgumentException("Must not be empty", nameof(text));

            #endregion

            switch (text.ToLower())
            {
                case "amzn" : return Amazon;
                case "appl" : return Apple;
                case "goog" : return Google;
                case "msft" : return Microsoft;
                case "orcl" : return Oracle;
                          
                // Full names
                           
                case "amazon"    : return Amazon;
                case "apple"     : return Apple;
                case "google"    : return Google;
                case "microsoft" : return Microsoft;
                case "oracle"    : return Oracle;

                default: throw new Exception("Unexpected resource identifier: " + text);
            } 
        }
    }
}

// amzn:instance:i-07e6001e0415497e4
// goog:
// amzn:
// msft: