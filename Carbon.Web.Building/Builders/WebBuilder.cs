using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

using TypeScript;

namespace Carbon.Builder
{
    using Css;
    using Data;
    using Logging;
    using Storage;
    using Packaging;
    using Building;

    public class WebBuilder
    {
        private readonly TypeScriptCompiler typescript;

        private readonly Package package;
   
        private readonly string basePath;

        private readonly string buildId;

        private readonly ILogger log;

        private readonly CssResolver cssResolver;
        private readonly IBucket fs;

        public WebBuilder(ILogger log, Package package, IBucket fs)
        {
            #region Preconditions

            if (package == null) throw new ArgumentNullException(nameof(package));
            if (fs == null)      throw new ArgumentNullException(nameof(fs));

            #endregion

            this.log = log;
            this.fs = fs;
            this.package = package;
            this.buildId = DateTime.UtcNow.ToString("yyyyMMddHHmmss") + "-" + HexString.FromBytes(Guid.NewGuid().ToByteArray()).Substring(0, 16);
            this.basePath = $@"D:/builds/{buildId}/";

            this.typescript = new TypeScriptCompiler(this.basePath);

            this.cssResolver = new CssResolver(basePath, package);
        }

        // Builds a project to the provided file system, package, etc

        public async Task<BuildResult> BuildAsync()
        {
            var sw = Stopwatch.StartNew();

            var result = new BuildResult();

            var hasTypeScript = false;

            var assets = package.ToArray();

            // TODO: Copy over typescript files
            foreach (var file in assets)
            {
                hasTypeScript = true;

                if (file.Name.EndsWith(".ts") || file.Name == "tsconfig.json")
                {
                    var tempSourcePath = basePath + file.Name;

                    using (var stream = file.Open())
                    {
                        // This helper creates the directory if it exists
                        await stream.CopyToFileAsync(tempSourcePath).ConfigureAwait(false);
                    }

                    hasTypeScript = true;
                }
            }

            if (hasTypeScript)
            {
                await typescript.BuildAsync().ConfigureAwait(false);
            }

            foreach (var item in assets)
            {
                if (item.Name.EndsWith(".scss"))
                {
                    var compiledName = item.Name.Replace(".scss", ".css");

                    using (var compiled = await CompileCssAsync(item).ConfigureAwait(false))
                    {
                        if (compiled == null)
                        {
                            log.Info($"Skipped '{compiledName}' (is partial)");

                            continue;
                        }

                        log.Info($"Compiled '{compiledName}'");

                        await fs.PutAsync(compiledName, compiled).ConfigureAwait(false);
                    }
                }
                else if (item.Name.EndsWith(".ts"))
                {
                    var compiledName = item.Name.Replace(".ts", ".js");

                    var outputPath = basePath + compiledName;

                    var blob = Blob.FromFile(outputPath);

                    await fs.PutAsync(compiledName, blob);

                    // TODO: Add hook for testing

                }
                else if (item.IsStatic())
                {
                    await fs.PutAsync(item.Name, await ToBlob(item).ConfigureAwait(false)).ConfigureAwait(false);
                }
            }

            result.Elapsed = sw.Elapsed;

            return result;
        }

        #region Compilers

        private async Task<Blob> CompileCssAsync(IBlob file)
        {
            var sourceText = await file.ReadStringAsync().ConfigureAwait(false);

            if (sourceText == null || sourceText.Length == 0)
            {
                throw new Exception("source file is empty");
            }

            if (sourceText.StartsWith("//= partial")) return null;

            var basePath = "/" + file.Name.Replace(Path.GetFileName(file.Name), "");

            var output = new MemoryStream();

            // StreamWriter.Dispose() closes the underlying stream. DO NOT DISPOSE

            var writer = new StreamWriter(output, Encoding.UTF8);

            var sheet = StyleSheet.Parse(sourceText, new CssContext());

            sheet.SetResolver(cssResolver);

            sheet.WriteTo(writer);

            writer.Flush();

            output.Position = 0;

            return new Blob(output) {
                ContentType = "text/css"
            };
        }

        #endregion

        public void Dispose()
        {
            // Delete the build folder
            Directory.Delete(basePath, recursive: true);
        }

        #region Helpers

        private static async Task<Blob> ToBlob(IBlob asset)
        {
            var ms = new MemoryStream();

            using (var s = asset.Open())
            {
                await s.CopyToAsync(ms).ConfigureAwait(false);
            }

            ms.Position = 0;
            
            return new Blob(ms);
        }

        #endregion
    }

    public static class StreamExtensions
    {
        public static async Task CopyToFileAsync(this Stream stream, string destinationFilePath)
        {
            #region Preconditions

            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            if (destinationFilePath == null)
                throw new ArgumentNullException(nameof(destinationFilePath));

            #endregion

            #region Ensure the directory exists

            var di = new DirectoryInfo(Path.GetDirectoryName(destinationFilePath));

            if (!di.Exists) di.Create();

            #endregion

            using (var writeStream = new FileStream(destinationFilePath, FileMode.CreateNew))
            {
                await stream.CopyToAsync(writeStream).ConfigureAwait(false);
            }
        }
    }
}

