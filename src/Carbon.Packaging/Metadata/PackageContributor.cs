using System.Runtime.Serialization;

namespace Carbon.Packaging
{
    public class PackageContributor
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "email")]
        public string Email { get; set; }

        [DataMember(Name = "url")]
        public string Url { get; set; }

        public string Text { get; set; }

        /*
        public static PackageContributor Parse(string text)
        {

        }
        */
    }
}

// TODO: Shorthand format
// Barney Rubble <b@rubble.com> (http://barnyrubble.tumblr.com/)