using System;
using System.Collections.Generic;

namespace Carbon.Platform
{
    public class ResourceProvider
    {
        public static readonly ResourceProvider Amazon    = new ResourceProvider(1  , "aws",       "Amazon");
        public static readonly ResourceProvider Borg      = new ResourceProvider(2  , "borg",      "Borg");
        public static readonly ResourceProvider Google    = new ResourceProvider(3  , "google",    "Google");
        public static readonly ResourceProvider IBM       = new ResourceProvider(4  , "ibm",       "IBM");
        public static readonly ResourceProvider Microsoft = new ResourceProvider(5  , "azure",     "Microsoft"); // Microsoft Azure
        public static readonly ResourceProvider Oracle    = new ResourceProvider(6  , "oracle",    "Oracle");
        


        // Email Delivery Providers
        public static readonly ResourceProvider Postmark = new ResourceProvider(60, "postmark", "Postmark");

        // Payment Processors
        public static readonly ResourceProvider PayPal   = new ResourceProvider(120, "paypal", "PayPal");
        public static readonly ResourceProvider Stripe   = new ResourceProvider(130, "stripe", "Stripe");

      

        // Code Repository Providers = 1000
        // TODO: Line these ids up with Carbon.Repositories

        public static readonly ResourceProvider Bitbucket = new ResourceProvider(1000, "bitbucket", "Bitbucket");
        public static readonly ResourceProvider GitHub    = new ResourceProvider(1001, "github", "GitHub");

        // Certificates
        public static readonly ResourceProvider LetEncrypt = new ResourceProvider(2000, "letsencrypt", "Let’s Encrypt");

        public static readonly Dictionary<int, ResourceProvider> map = new Dictionary<int, ResourceProvider> {
            { 1,    Amazon },
            { 2,    Borg },
            { 3,    Google },
            { 4,    IBM },
            { 5,    Microsoft },
            { 6,    Oracle },
           
            { 120,  PayPal },
            { 130,  Stripe },

            { 1000, Bitbucket },
            { 1001, GitHub },

            { 2000,  LetEncrypt },
        };

        // GitHub
        // BitBucket
        // Dropbox

        public ResourceProvider(int id, string code, string name)
        {
            Id   = id;
            Code = code ?? throw new ArgumentNullException(nameof(code));
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public int Id { get; }

        public string Name { get; }

        public string Code { get; }

        public override string ToString() => Code;

        public static ResourceProvider Get(int id)
        {
            return map[id];
        }

        public static ResourceProvider Parse(string text)
        {
            #region Preconditions

            if (text == null)
                throw new ArgumentNullException(nameof(text));

            if (text.Length == 0)
                throw new ArgumentException("Must not be empty", nameof(text));

            #endregion

            switch (text.ToLower())
            {
                case "aws"       : return Amazon;
                case "amzn"      : return Amazon;
                case "borg"      : return Borg;
                case "ibm"       : return IBM;
                case "goog"      : return Google;
                case "msft"      : return Microsoft;
                case "azure"     : return Microsoft;
                case "orcl"      : return Oracle;

                // Full names
                case "bitbucket"  : return Bitbucket;
                case "github"     : return GitHub;
                case "amazon"     : return Amazon;
                case "google"     : return Google;
                case "microsoft"  : return Microsoft;
                case "oracle"     : return Oracle;

                case "letsencrypt": return LetEncrypt;

                default: throw new Exception("Unexpected provider: " + text);
            } 
        }
    }
}

// aws:instance:i-07e6001e0415497e4
// google:
// azure:


/*
public static int GetId(string text)
{
    var bytes = Encoding.UTF8.GetBytes(text.ToUpper().PadRight(4));

    Array.Reverse(bytes);

    return BitConverter.ToInt32(bytes, 0);
}
*/
