using System;
using System.Collections.Generic;

using System.Threading.Tasks;

namespace Carbon.Packaging
{
    using Data;
    using Data.Annotations;

    public class PackageMetadata
    {
        [Member(1)] // Required
        public string Name { get; set; }
        
        [Member(2)] // Required
        public Semver Version { get; set; }

        [Member(3), Optional]
        public string Description { get; set; }

        [Optional]
        public PackageContributor[] Contributors { get; set; }

        [Optional]
        public string Main { get; set; }

        [Optional]
        public string License { get; set; }

        [Optional]
        public IList<PackageDependencyInfo> Dependencies { get; } = new List<PackageDependencyInfo>();

        [Optional]
        public PackageRepository Repository { get; set; }

        [Optional]
        public string[] Files { get; set; }

        public static PackageMetadata Parse(string text)
        {
            var json = XObject.Parse(text);

            var metadata = new PackageMetadata {
                Name = json["name"]
            };

            if (json.ContainsKey("version"))
            {
                metadata.Version = Semver.Parse(json["version"]);
            }

            if (json.ContainsKey("main"))
            {
                metadata.Main = json["main"];
            }

            if (json.ContainsKey("repository"))
            {
                var repositoryNode = json["repository"];

                if (repositoryNode.Type == XType.String)
                {
                    metadata.Repository = new PackageRepository(RepositoryType.Git, repositoryNode);
                }
            }

            if (json.ContainsKey("dependencies"))
            {
                foreach (var pair in (XObject)json["dependencies"])
                {
                    var dep = new PackageDependencyInfo(pair.Key, pair.Value);

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

        public static async Task<PackageMetadata> Load(IFile file)
            => Parse(await file.ReadStringAsync().ConfigureAwait(false));
    }

    public struct PackageDependencyInfo
    {
        public PackageDependencyInfo(string name, string text)
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

        public Semver Version => Semver.Parse(Value);
    }

    /*
    { 
      "type" : "git",
      "url" : "https://github.com/npm/npm.git"
    }
    */

    public struct PackageRepository
    {
        public PackageRepository(RepositoryType type, string url)
        {
            Type = type;
            Url = url;
        }

        public RepositoryType Type { get; }

        public string Url { get; }
    }

    public class PackageContributor
    {
        public string Name { get; set; }

        public string Email { get; set; }
    }
}