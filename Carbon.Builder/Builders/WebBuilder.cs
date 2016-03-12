namespace Carbon.Builder
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using System.Linq;
    using System.Threading.Tasks;

    using Carbon.Css;
    using Carbon.Platform;
    using Carbon.Logging;
    using Carbon.Helpers;

    using TypeScript;
    using Storage;

    public class WebBuilder
    {
        private readonly TypeScriptCompiler typescript;

        private readonly Package package;
   
        private readonly string basePath;

        private readonly string buildId;

        private readonly ILogger log;

        private readonly CssResolver cssResolver;
        private readonly IBlobStore fs;

        public WebBuilder(ILogger log, Package package, IBlobStore fs)
        {
            #region Preconditions

            if (package == null) throw new ArgumentNullException(nameof(package));
            if (fs == null)      throw new ArgumentNullException(nameof(fs));

            #endregion

            this.log = log;
            this.fs = fs;
            this.package = package;
            this.buildId = new Guid().ToString();
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

        private async Task<Blob> CompileCssAsync(IAsset asset)
        {
            var sourceText = await asset.ReadStringAsync().ConfigureAwait(false);

            if (sourceText == null || sourceText.Length == 0)
            {
                throw new Exception("source file is empty");
            }

            if (sourceText.StartsWith("//= partial")) return null;

            var basePath = "/" + asset.Name.Replace(Path.GetFileName(asset.Name), "");

            var output = new MemoryStream();

            // StreamWriter.Dispose() closes the underlying stream. DO NOT DISPOSE

            var writer = new StreamWriter(output, Encoding.UTF8);

            var sheet = StyleSheet.Parse(sourceText, new CssContext());

            sheet.SetResolver(cssResolver);

            sheet.WriteTo(writer);

            writer.Flush();

            output.Position = 0;

            return new Blob(output) {
                Metadata = {
                    ["Content-Type"] = "text/css"
                }
            };
        }

        #endregion

        public void Dispose()
        {
            // Delete the build folder
            Directory.Delete(basePath, recursive: true);
        }

        #region Helpers

        private static async Task<Blob> ToBlob(IAsset asset)
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

}

