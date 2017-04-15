using System;
using System.Collections.Generic;

namespace Carbon.Platform
{
    public class ResourceProvider
    {
        // Cloud Platforms (1-255)
        public static readonly ResourceProvider Borg         = new ResourceProvider(0  , "borg",         "Borg");
        public static readonly ResourceProvider Amazon       = new ResourceProvider(1  , "aws",          "Amazon");
        public static readonly ResourceProvider Google       = new ResourceProvider(2  , "google",       "Google");
        public static readonly ResourceProvider IBM          = new ResourceProvider(3  , "ibm",          "IBM");
        public static readonly ResourceProvider Microsoft    = new ResourceProvider(4  , "azure",        "Microsoft");
        public static readonly ResourceProvider DigitalOcean = new ResourceProvider(20 , "digitalocean", "DigitalOcean");
     
        // Code Repository Providers = 1000
        public static readonly ResourceProvider GitHub    = new ResourceProvider(1000, "github",    "GitHub",    "github.com");
        public static readonly ResourceProvider Bitbucket = new ResourceProvider(1001, "bitbucket", "Bitbucket", "bitbucket.org");
        public static readonly ResourceProvider GitLab    = new ResourceProvider(1002, "gitlab",    "GitLab",    "gitlab.com");

        // Payment Processors = 2000
        public static readonly ResourceProvider Braintree = new ResourceProvider(2000, "braintree", "Braintree");
        public static readonly ResourceProvider PayPal    = new ResourceProvider(2001, "paypal",    "PayPal", "paypal.com");
        public static readonly ResourceProvider Stripe    = new ResourceProvider(2002, "stripe",    "Stripe", "stripe.com");

        // Certificates = 3000
        public static readonly ResourceProvider LetEncrypt = new ResourceProvider(3000, "letsencrypt", "Let’s Encrypt");

        // Email Delivery Providers = 5000
        public static readonly ResourceProvider Postmark = new ResourceProvider(5000, "postmark", "Postmark");

        public static readonly Dictionary<int, ResourceProvider> map = new Dictionary<int, ResourceProvider> {
            { 0,    Borg },
            { 1,    Amazon },
            { 2,    Google },
            { 3,    IBM },
            { 4,    Microsoft },
            { 20,   DigitalOcean },

            { 2000,  Braintree },
            { 2001,  PayPal },
            { 2002,  Stripe },

            { 1000,  GitHub },
            { 1001,  Bitbucket },
            { 1002,  GitLab },

            { 3000,  LetEncrypt },
            { 5000,  Postmark }
        };

        // GitHub
        // BitBucket
        // Dropbox

        public ResourceProvider(int id, string code, string name, string domain = null)
        {
            Id     = id;
            Code   = code ?? throw new ArgumentNullException(nameof(code));
            Name   = name ?? throw new ArgumentNullException(nameof(name));
            Domain = domain;
        }

        public int Id { get; }

        public string Name { get; }

        public string Domain { get; }

        public string Code { get; }

        public override string ToString() => Code;

        public static ResourceProvider FromLocationId(int locationId)
        {
            var providerId = LocationId.Create(locationId).ProviderId;

            return Get(providerId);
        }

        public static ResourceProvider Get(int id)
        {
            return map.TryGetValue(id, out var value) ? value : throw new Exception($"No provider#{id}");
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
                case "aws"          : return Amazon;
                case "amzn"         : return Amazon;
                case "borg"         : return Borg;
                case "ibm"          : return IBM;
                case "goog"         : return Google;
                case "msft"         : return Microsoft;
                case "azure"        : return Microsoft;

                // Full names
                case "bitbucket"    : return Bitbucket;
                case "github"       : return GitHub;
                case "amazon"       : return Amazon;
                case "google"       : return Google;
                case "microsoft"    : return Microsoft;

                case "braintree"    : return Braintree;
                case "paypal"       : return PayPal;
                case "stripe"       : return Stripe;
                
                case "postmark"     : return Postmark;
                case "letsencrypt"  : return LetEncrypt;

                default: throw new Exception("Unexpected provider: " + text);
            } 
        }
    }
}

// aws:instance:i-07e6001e0415497e4
// google:
// azure: