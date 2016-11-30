using System.Runtime.Serialization;
using System.IO;
using System.Collections.Generic;

namespace Carbon.Packaging
{
    using Json;
    using Storage;
    using Versioning;

    public class PackageMetadata : IPackage
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "version")]
        public SemanticVersion Version { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "author")]
        public PackageContributor Author { get; set; }

        [DataMember(Name = "contributors")]
        public PackageContributor[] Contributors { get; set; }

        [DataMember(Name = "main")]
        public string Main { get; set; }

        public string License { get; set; }

        public IList<PackageDependency> Dependencies { get; } = new List<PackageDependency>();

        [DataMember(Name = "repository")]
        public PackageRepository Repository { get; set; }
    
        public string[] Files { get; set; }

        public static PackageMetadata Parse(string text)
        {
            var json = JsonObject.Parse(text);

            var metadata = new PackageMetadata {
                Name = json["name"]
            };

            if (json.ContainsKey("version"))
            {
                metadata.Version = SemanticVersion.Parse(json["version"]);
            }

            if (json.ContainsKey("main"))
            {
                metadata.Main = json["main"];
            }

            if (json.ContainsKey("repository"))
            {
                var repository = json["repository"];

                if (repository is JsonObject)
                {
                    /*
                    { 
                      "type" : "git",
                      "url" : "https://github.com/npm/npm.git"
                    }
                    */

                    metadata.Repository = repository.As<PackageRepository>();
                }
                else
                {
                    metadata.Repository = PackageRepository.Parse(repository);
                }
            }

            if (json.ContainsKey("dependencies"))
            {
                foreach (var pair in (JsonObject)json["dependencies"])
                {
                    var dep = new PackageDependency(pair.Key, pair.Value);

                    metadata.Dependencies.Add(dep);
                }
            }
   
            if (json.ContainsKey("files"))
            {
                metadata.Files = json["files"].ToArrayOf<string>();
            }

            if (json.ContainsKey("author"))
            {
                metadata.Author = json["author"].As<PackageContributor>();
            }

            if (json.ContainsKey("contributors"))
            {
                metadata.Contributors = json["contributors"].ToArrayOf<PackageContributor>();
            }

            return metadata;
        }

        public static PackageMetadata Parse(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                var text = reader.ReadToEnd();

                return Parse(text);
            }
        }

        public static PackageMetadata FromBlob(IBlob file)
            => Parse(file.Open());
    }
}