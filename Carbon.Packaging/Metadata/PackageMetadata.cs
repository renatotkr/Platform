using System;
using System.Collections.Generic;

using System.Threading.Tasks;

namespace Carbon.Packaging
{
    using Data;
    using Data.Annotations;

    public class PackageMetadata
    {
        // Required
        public string Name { get; set; }

        // Required
        public Semver Version { get; set; }

        [Optional]
        public string Description { get; set; }

        [Optional]
        public PackageContributor[] Contributors { get; set; }

        [Optional]
        public string Main { get; set; }

        [Optional]
        public string License { get; set; }

        [Optional]
        public IList<PackageDependency> Dependencies { get; } = new List<PackageDependency>();

        [Optional]
        public PackageRepository Repository { get; set; }

        [Optional]
        public string[] Files { get; set; }

        public static PackageMetadata Parse(string text)
        {
            var json = XObject.Parse(text);

            var metadata = new PackageMetadata {
                Name  = (string) json["name"]
            };

            if (json.ContainsKey("version"))
            {
                metadata.Version = Semver.Parse(json["version"].ToString());
            }

            if (json.ContainsKey("main"))
            {
                metadata.Main = json["main"].ToString();
            }

            if (json.ContainsKey("repository"))
            {
                var repositoryNode = json["repository"];

                if (repositoryNode.Type == XType.String)
                {
                    metadata.Repository = new PackageRepository {
                        Url = repositoryNode.ToString()
                    };
                }
            }

            if (json.ContainsKey("dependencies"))
            {
                foreach (var pair in (XObject)json["dependencies"])
                {
                    var dep = new PackageDependency(pair.Key, pair.Value.ToString());

                    metadata.Dependencies.Add(dep);
                }
            }
   
            if (json.ContainsKey("files"))
            {
                metadata.Files = ((XArray)json["files"]).ToArrayOf<string>();
            }

            if (json.ContainsKey("contributors"))
            {
                metadata.Contributors = ((XArray)json["contributors"]).ToArrayOf<PackageContributor>();
            }

            return metadata;
        }

        public static async Task<PackageMetadata> FromAsset(IFile asset)
            => Parse(await asset.ReadStringAsync().ConfigureAwait(false));
    }

    public class PackageDependency
    {
        public PackageDependency(string name, string text)
        {
            #region Preconditions

            if (name == null) throw new ArgumentNullException(nameof(name));
            if (text == null) throw new ArgumentNullException(nameof(text));

            #endregion

            Name = name;
            Value = text;
        }

        public string Name { get; }

        public string Value { get; }

        public bool IsFile => !char.IsDigit(Value[0]);

        public SemverRange Version => Semver.Parse(Value).GetRange();
    }

    /*
    { 
      "type" : "git",
      "url" : "https://github.com/npm/npm.git"
    }
    */

    public class PackageRepository
    {
        public string Type { get; set; }

        public string Url { get; set; }
    }

    public class PackageContributor
    {
        public string Name { get; set; }

        public string Email { get; set; }
    }
}
