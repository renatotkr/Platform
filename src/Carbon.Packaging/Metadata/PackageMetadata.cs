using System.Runtime.Serialization;
using System.IO;
using System.Collections.Generic;

namespace Carbon.Packaging
{
    using Json;
    using Storage;
    using Versioning;

    public class PackageMetadata
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

        public List<PackageDependency> Dependencies { get; } = new List<PackageDependency>();

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
                // Your Name <you.name@example.org>

                if (json["author"] is JsonString)
                {
                    metadata.Author = new PackageContributor { Text = json["author"] };
                }
                else
                {
                    metadata.Author = json["author"].As<PackageContributor>();
                }
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
                return Parse(reader.ReadToEnd());
            }
        }

        public static PackageMetadata FromBlob(IBlob file)
        {
            return Parse(file.OpenAsync().Result);
        }
    }
}



/*
NPM Description:
A package is any of:

a) a folder containing a program described by a package.json file
b) a gzipped tarball containing(a)
c) a url that resolves to(b)
d) a <name>@<version> that is published on the registry with(c)
e) a <name>@<tag> that points to(d)
f) a <name> that has a latest tag satisfying(e)
g) a git url that, when cloned, results in (a).
*/
