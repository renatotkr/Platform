using System.Runtime.Serialization;

namespace Carbon.Packaging
{
    [DataContract]
    public class PackageContributor
    {
        public PackageContributor() { }

        public PackageContributor(
            string name, 
            string email = null,
            string url = null)
        {
            Name = name;
            Email = email;
            Url = url;
        }

        [DataMember(Name = "name", Order = 1)]
        public string Name { get; set; }

        [DataMember(Name = "email", Order = 2)]
        public string Email { get; set; }

        [DataMember(Name = "url", Order = 3)]
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