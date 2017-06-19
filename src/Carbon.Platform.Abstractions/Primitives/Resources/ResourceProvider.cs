using System;
using System.Collections.Generic;

namespace Carbon.Platform
{
    public class ResourceProvider
    {
        // Cloud Platforms (1-255)      
        public static readonly ResourceProvider Borg  = new ResourceProvider(1, "borg",  "Borg",  "borg.cloud");
        public static readonly ResourceProvider Aws   = new ResourceProvider(2, "aws",   "AWS",   "amazonaws.com");
        public static readonly ResourceProvider Gcp   = new ResourceProvider(3, "gcp",   "GCP",   "cloud.google.com");
        public static readonly ResourceProvider Azure = new ResourceProvider(4, "azure", "Azure", "azure.microsoft.com");

        // Banks
        public static readonly ResourceProvider Braintree = new ResourceProvider(5001, "braintree", "Braintree", "braintreepayments.com");
        public static readonly ResourceProvider PayPal    = new ResourceProvider(5000, "paypal",    "PayPal", "paypal.com");
        public static readonly ResourceProvider Stripe    = new ResourceProvider(5002, "stripe",    "Stripe", "stripe.com");

        // Code Repository Providers 
        public static readonly ResourceProvider GitHub    = new ResourceProvider(6000, "github", "GitHub", "github.com");
        public static readonly ResourceProvider Bitbucket = new ResourceProvider(6001, "bitbucket", "Bitbucket", "bitbucket.org");

        // Certificates = 3000
        // public static readonly ResourceProvider LetEncrypt = new ResourceProvider(3000, "letsencrypt", "Let’s Encrypt");

        // public static readonly ResourceProvider Postmark = new ResourceProvider(5000, "postmark", "Postmark");

        public static readonly Dictionary<int, ResourceProvider> map = new Dictionary<int, ResourceProvider> {
            { 1,    Borg },
            { 2,    Aws },
            { 3,    Gcp },
            { 4,    Azure },

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
            #region Preconditions

            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (name.Length == 0)
                throw new ArgumentException("Must not be empty", nameof(name));

            #endregion

            switch (name.ToLower())
            {
                case "borg"        : return Borg;
                case "aws"         : return Aws;
                case "gcp"         : return Gcp;
                case "azure"       : return Azure;

                // Full names
                case "bitbucket"   : return Bitbucket;
                case "github"      : return GitHub;

                case "braintree"   : return Braintree;
                case "paypal"      : return PayPal;
                case "stripe"      : return Stripe;

                default: throw new Exception($"Unexpected provider '{name}'");
            } 
        }
    }
}