using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Carbon.Json;
using Carbon.Storage;
using Carbon.Versioning;

namespace Carbon.Packaging
{
    public class PackageMetadata
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "version")]
        public SemanticVersion Version { get; set; }

        [DataMember(Name = "description", EmitDefaultValue = false)]
        public string Description { get; set; }

        [DataMember(Name = "author", EmitDefaultValue = false)]
        public PackageContributor Author { get; set; }

        [DataMember(Name = "contributors", EmitDefaultValue = false)]
        public PackageContributor[] Contributors { get; set; }

        [DataMember(Name = "main", EmitDefaultValue = false)]
        public string Main { get; set; }

        // [DataMember(Name = "license", EmitDefaultValue = false)]
        public string License { get; set; }

        public PackageDependency[] Dependencies { get; private set; }

        [DataMember(Name = "repository", EmitDefaultValue = false)]
        public PackageRepository Repository { get; set; }
    
        public string[] Files { get; set; }

        public static PackageMetadata Parse(string text)
        {
            var json = JsonObject.Parse(text);

            var metadata = new PackageMetadata {
                Name = json["name"]
            };

            if (json.TryGetValue("version", out var versionNode))
            {
                metadata.Version = SemanticVersion.Parse(versionNode);
            }

            if (json.TryGetValue("main", out var mainNode))
            {
                metadata.Main = mainNode;
            }

            if (json.TryGetValue("repository", out var repositoryNode))
            {
                if (repositoryNode is JsonObject)
                {
                    /*
                    { 
                      "type" : "git",
                      "url" : "https://github.com/npm/npm.git"
                    }
                    */

                    metadata.Repository = repositoryNode.As<PackageRepository>();
                }
                else
                {
                    metadata.Repository = PackageRepository.Parse(repositoryNode);
                }
            }

            if (json.TryGetValue("dependencies", out var dependenciesNode) 
                && dependenciesNode is JsonObject dependenciesObject)
            {
                var deps = new PackageDependency[dependenciesObject.Values.Count];

                var i = 0;

                foreach (var pair in dependenciesObject)
                {
                    deps[i] = new PackageDependency(pair.Key, pair.Value);

                    i++;
                }

                metadata.Dependencies = deps;
            }
   
            if (json.TryGetValue("files", out var filesNode))
            {
                metadata.Files = filesNode.ToArrayOf<string>();
            }

            if (json.TryGetValue("author", out var authorNode))
            {
                // Your Name <you.name@example.org>

                if (authorNode is JsonString)
                {
                    metadata.Author = new PackageContributor { Text = authorNode };
                }
                else
                {
                    metadata.Author = authorNode.As<PackageContributor>();
                }
            }

            if (json.TryGetValue("contributors", out var contributorsNode))
            {
                metadata.Contributors = contributorsNode.ToArrayOf<PackageContributor>();
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

        public static async Task<PackageMetadata> ParseAsync(IBlob file)
        {
            var stream = await file.OpenAsync().ConfigureAwait(false);

            // dispose?

            return Parse(stream);
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
