using System.Runtime.Serialization;

namespace Carbon.Packaging
{
    [DataContract]
    public class PackageRepository
    {
        [DataMember(Name = "type", Order = 1, EmitDefaultValue = false)]
        public string Type { get; set; }
        
        [DataMember(Name = "url", Order = 2)]
        public string Url { get; set; }
        
        [IgnoreDataMember]
        public string Text { get; set; }

        public static implicit operator string(PackageRepository repository)
        {
            return repository.ToString();
        }

        public override string ToString() => Text ?? Url;

        public static PackageRepository Parse(string text) => 
            new PackageRepository { Text = text };
    }

    /*
    "repository": "npm/npm"

    "repository": "gist:11081aaa281"

    "repository": "bitbucket:example/repo"

    "repository": "gitlab:another/repo"
    */
}
 