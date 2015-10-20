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

    public class WebBuilder
    {
        private static readonly TypeScriptCompiler compiler = new TypeScriptCompiler();

        private readonly Package package;
   
        private readonly string basePath;

        private readonly string buildId;

        private readonly ILogger log;

        private readonly CssPackageResolver cssResolver;

        public WebBuilder(ILogger log, Package package)
        {
            #region Preconditions

            if (package == null) throw new ArgumentNullException(nameof(package));

            #endregion

            this.log = log;
            this.package = package;
            this.buildId = new Guid().ToString();
            this.basePath = $@"D:/builds/{buildId}/";

            this.cssResolver = new CssPackageResolver(basePath, package);
        }

        // Builds a project to the provided file system, package, etc

        public async Task<BuildResult> BuildAsync(IFileSystem fs)
        {
            var sw = Stopwatch.StartNew();
            var result = new BuildResult();

            var assets = package.ToArray();

            // TODO: Copy over typescript files
            foreach (var file in assets)
            {
                if (file.Name.EndsWith(".ts") || file.Name == "tsconfig.json")
                {
                    var tempSourcePath = basePath + file.Name;

                    using (var stream = file.Open())
                    {
                        // This helper creates the directory if it exists
                        await stream.CopyToFileAsync(tempSourcePath).ConfigureAwait(false);
                    }
                }
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

                        await fs.CreateAsync(compiledName, compiled).ConfigureAwait(false);
                    }
                       
                }
                else if (item.Name.EndsWith(".ts"))
                {
                    var compiledName = item.Name.Replace(".ts", ".js");

                    using (var compiled = await CompileTypeScriptAsync(item).ConfigureAwait(false))
                    {
                        log.Info($"Compiled '{compiledName}'");

                        await fs.CreateAsync(compiledName, compiled).ConfigureAwait(false);
                    }
                }
                else if (item.IsStatic())
                {
                    await fs.CreateAsync(item.Name, await ToMemoryStreamAsync(item).ConfigureAwait(false)).ConfigureAwait(false);
                }
              
            }

            result.Elapsed = sw.Elapsed;

            return result;
        }


        #region Compilers

        private async Task<MemoryStream> CompileTypeScriptAsync(IAsset asset)
        {
            var outputPath = basePath + asset.Name.Replace(".ts", ".js");

            // Skip if we've already built the file
            if (!File.Exists(outputPath))
            {
                await compiler.BuildAsync(basePath).ConfigureAwait(false);
            }

            var ms = new MemoryStream();

            using (var outputStream = File.Open(outputPath, FileMode.Open))
            {
                await outputStream.CopyToAsync(ms).ConfigureAwait(false);
            }

            ms.Position = 0;

            return ms;
        }

        private async Task<MemoryStream> CompileCssAsync(IAsset asset)
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

            return output;
        }


        #endregion

        public void Dispose()
        {
            // Delete the build folder
            Directory.Delete(basePath, recursive: true);
        }

        #region Helpers

        private static async Task<MemoryStream> ToMemoryStreamAsync(IAsset asset)
        {
            var ms = new MemoryStream();

            using (var s = asset.Open())
            {
                await s.CopyToAsync(ms).ConfigureAwait(false);
            }

            ms.Position = 0;

            return ms;
        }

        #endregion
    }

}

