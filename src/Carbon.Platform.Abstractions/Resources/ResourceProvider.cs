using System;
using System.Collections.Generic;

namespace Carbon.Platform
{
    public class ResourceProvider
    {
        // Clouds (1-255)      
        public static readonly ResourceProvider Borg         = new ResourceProvider(1,  "borg",  "Borg",  "borg.cloud");
        public static readonly ResourceProvider Aws          = new ResourceProvider(2,  "aws",   "AWS",   "amazonaws.com");
        public static readonly ResourceProvider Gcp          = new ResourceProvider(3,  "gcp",   "GCP",   "cloud.google.com");
        public static readonly ResourceProvider Azure        = new ResourceProvider(4,  "azure", "Azure", "azure.microsoft.com");
        public static readonly ResourceProvider IBM          = new ResourceProvider(5,  "ibm",   "IBM",   "ibm.com"); // Formally softlayer

        public static readonly ResourceProvider DigitalOcean = new ResourceProvider(10,  "digitalocean", "Digital Ocean", "digitalocean.com");
        public static readonly ResourceProvider Vultr        = new ResourceProvider(20,  "vultr",  "Vultr", "vultr.com");
        
        public static readonly ResourceProvider GCore        = new ResourceProvider(105, "gcore",  "GCore",  "gcore.lu");
        public static readonly ResourceProvider Incero       = new ResourceProvider(106, "incero", "Incero", "incero.com");
        public static readonly ResourceProvider Wasabi       = new ResourceProvider(107, "wasabi", "Wasabi", "wasabi.com");

        // < 127

        // Banks`
        public static readonly ResourceProvider PayPal    = new ResourceProvider(5000, "paypal", "PayPal", "paypal.com");
        public static readonly ResourceProvider Braintree = new ResourceProvider(5001, "braintree", "Braintree", "braintreepayments.com");
        public static readonly ResourceProvider Stripe    = new ResourceProvider(5002, "stripe",    "Stripe", "stripe.com");

        // Code Repository Providers 
        public static readonly ResourceProvider GitHub    = new ResourceProvider(6000, "github", "GitHub", "github.com");
        public static readonly ResourceProvider Bitbucket = new ResourceProvider(6001, "bitbucket", "Bitbucket", "bitbucket.org");

        // TODO: Let's Encrypt

        public static readonly Dictionary<int, ResourceProvider> map = new Dictionary<int, ResourceProvider> {
            { 1,    Borg },
            { 2,    Aws },
            { 3,    Gcp },
            { 4,    Azure },
            { 5,    IBM },
            { 10,   DigitalOcean },
            { 20,   Vultr },
            { 105,  GCore },
            { 106,  Incero },
            { 107,  Wasabi },

            { 5000,  PayPal },
            { 5001,  Braintree },
            { 5002,  Stripe },

            { 6000,  GitHub },
            { 6001,  Bitbucket }
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

        public static ResourceProvider Parse(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (name.Length == 0)
                throw new ArgumentException("Must not be empty", nameof(name));

            switch (name.ToLower())
            {
                case "borg"         : return Borg;         // 1
                case "aws"          : return Aws;          // 2
                case "gcp"          : return Gcp;          // 3
                case "azure"        : return Azure;        // 4
                case "ibm"          : return IBM;          // 5
                case "digitalocean" : return DigitalOcean; // 10
                case "vultr"        : return Vultr;        // 20
                case "gcore"        : return GCore;        // 105
                case "incero"       : return Incero;       // 106
                case "wasabi"       : return Wasabi;       // 107

                // Full names    
                case "bitbucket" : return Bitbucket;
                case "github"    : return GitHub;
                                 
                case "braintree" : return Braintree;
                case "paypal"    : return PayPal;
                case "stripe"    : return Stripe;
            }

            throw new Exception($"Invalid provider name '{name}'");
        }
    }
}