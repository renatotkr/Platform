using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using Carbon.Packaging;
using Carbon.Platform.Web;
using Carbon.Protection;
using Carbon.Storage;
using Carbon.VersionControl;

namespace Carbon.Platform.CI
{
    public class WebLibraryDeployer
    {
        private readonly IBucket bucket;

        public WebLibraryDeployer(IBucket bucket)
        {
            this.bucket = bucket ?? throw new ArgumentNullException(nameof(bucket));
        }

        public PackageMetadata GetMetadata(IPackage package)
        {
            #region Preconditions

            if (package == null)
                throw new ArgumentNullException(nameof(package));

            #endregion

            var metadataFile = package.Find("package.json");

            if (metadataFile == null)
            {
                throw new Exception("Missing /package.json");
            }

            return PackageMetadata.Parse(metadataFile.OpenAsync().Result);
        }

        public async Task<WebLibrary> DeployAsync(IPackage package, RevisionSource source, int publisherId)
        {
            #region Preconditions

            if (package == null) throw new ArgumentNullException(nameof(package));

            #endregion

            var metadata = GetMetadata(package);

            var version = metadata.Version;

            // var existing = await registry.GetAsync(registry.Lookup(metadata.Name), metadata.Version);

            // if (existing != null) throw new PublishingConflict(existing);

            var mainPath = metadata.Main;

            var bowerFile = package.Find("bower.json");

            if (bowerFile != null)
            {
                try
                {
                    var bowerFileStream = await bowerFile.OpenAsync().ConfigureAwait(false);

                    var bower = PackageMetadata.Parse(bowerFileStream);

                    if (bower.Main != null)
                    {
                        mainPath = bower.Main;
                    }
                }
                catch { }
            }

            if (mainPath == null)
            {
                throw new Exception("A main property found in package.json or bower.json.");
            }

            var mainFile = package.Find(mainPath);

            if (mainFile == null)
            {
                throw new Exception($"The main file '{mainPath}' was not found");
            }

            var mainText = await mainFile.ReadAllTextAsync().ConfigureAwait(false);

            if (mainText.Length == 0) throw new Exception($"{mainPath} is empty");

            var mainBlobStream = new MemoryStream(Encoding.UTF8.GetBytes(mainText));

            var mainName = mainPath.Split('/').Last();

            if (!mainName.EndsWith(".js"))
            {
                throw new Exception($"Must end with js. was {mainName}");
            }

            var mainBlob = new Blob(
                name        : $"libs/{metadata.Name}/{version}/{mainName}",
                stream      : mainBlobStream,
                metadata    : new BlobMetadata {
                    ContentType = "application/javascript"
                }
            );

            var mainHash = Hash.ComputeSHA256(mainBlobStream);

            // Push to CDN
            await bucket.PutAsync(mainBlob).ConfigureAwait(false);

            if (metadata.Files != null)
            {
                // TODO: Copy over everything from files[] (excluding main)
                foreach (var fileName in metadata.Files)
                {
                    var fn = fileName;

                    if (fn.StartsWith("./"))
                    {
                        fn = fileName.Substring(2);
                    }

                    if (fn == mainPath) continue;

                    var asset = package.Find(fn);

                    var format = asset.Name.Split('.').Last();
                   
                    var n = asset.Name.Split('/').Last();

                    var ms = new MemoryStream();

                    using (var data = await asset.OpenAsync().ConfigureAwait(false))
                    {
                        data.CopyTo(ms);

                        ms.Position = 0;
                    }

                    var blob = new Blob(
                        name     : $"libs/{metadata.Name}/{version}/{n}",
                        stream   : ms,
                        metadata : new BlobMetadata {
                            ContentType = GetMime(format)
                        }
                    );

                    await bucket.PutAsync(blob).ConfigureAwait(false);
                }
            }

            var release = new WebLibrary(metadata.Name, version) {
                MainName   = mainName,
                MainSha256 = mainHash,
                Source     = source.ToString()
            };

            return release;
        }

        // TODO: Use Carbon.Media

        public string GetMime(string format)
        {
            switch (format)
            {
                case "LICENSE"  : return "text/plain";
                case "woff"     : return "application/font-woff";
                case "svg"      : return "image/svg+xml";
                case "js"       : return "application/javascript";
                case "json"     : return "application/json";
                case "css"      : return "text/css";
                case "png"      : return "image/png";
                case "jpg"      :
                case "jpeg"     : return "image/jpeg";
                case "gif"      : return "image/gif";
                case "swf"      : return "application/x-shockwave-flash";
                default         : throw new Exception("unexpected format:" + format);
            }
        }
    }
}
