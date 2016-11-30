using System.Runtime.Serialization;

namespace Carbon.Packaging
{
    public class PackageRepository
    {
        [DataMember(Name = "url")]
        public string Url { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }

        [IgnoreDataMember]
        public string Text { get; set; }

        public static implicit operator string(PackageRepository repository)
            => repository.ToString();

        public override string ToString()
            => Text ?? Url;

        public static PackageRepository Parse(string text)
            => new PackageRepository { Text = text };
    }

    /*
    "repository": "npm/npm"

    "repository": "gist:11081aaa281"

    "repository": "bitbucket:example/repo"

    "repository": "gitlab:another/repo"
    */
}
 