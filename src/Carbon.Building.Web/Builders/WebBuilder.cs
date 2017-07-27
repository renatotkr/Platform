using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Carbon.Css;
using Carbon.Logging;
using Carbon.Storage;

namespace Carbon.Building.Web
{
    public class WebBuilder
    {
        private readonly string basePath;
        private readonly string buildId;
        private readonly IPackage package;
        private readonly ILogger log;

        private readonly CssResolver cssResolver;
        private readonly IBucket bucket;

        private readonly TypeScriptProject tsProject;

        public WebBuilder(ILogger log, IPackage package, IBucket bucket, string baseDirectory = "D:/builds")
        {
            this.log     = log           ?? throw new ArgumentNullException(nameof(log));
            this.bucket  = bucket        ?? throw new ArgumentNullException(nameof(bucket));
            this.package = package       ?? throw new ArgumentNullException(nameof(package));

            var unique = Guid.NewGuid().ToString("N");

            this.buildId = DateTime.UtcNow.ToString("yyyyMMddHHmmss") + "-" + unique;
            this.basePath = baseDirectory + $@"/{buildId}/";

            this.tsProject = new TypeScriptProject(package, basePath);

            this.cssResolver = new CssResolver(basePath, package);
        }

        // Builds a project to the provided file system, package, etc

        public async Task<BuildResult> BuildAsync()
        {
            var ct = new CancellationTokenSource();

            var sw = Stopwatch.StartNew();

            var result = new BuildResult();

            bool compiledTs = false;

            foreach (var file in package)
            {
                var format = FormatHelper.GetFormat(file);

                if (format == "scss")
                {
                    using (var compiledCss = await CompileCssAsync(file).ConfigureAwait(false))
                    {
                        var compiledName = file.Name.Replace(".scss", ".css");

                        if (compiledCss == null)
                        {
                            log.Info($"Skipped '{compiledName}' (is partial)");

                            continue;
                        }

                        log.Info($"Compiled '{compiledName}'");

                        var blob = new Blob(compiledName, compiledCss, new BlobMetadata {
                            ContentType = "text/css"
                        });

                        await bucket.PutAsync(blob).ConfigureAwait(false);
                    }
                }
                else if (format == "ts")
                {
                    if (!compiledTs)
                    {
                        log.Info("Building TypeScript");

                        // This will copy over the typescript files
                        await tsProject.BuildAsync(ct.Token).ConfigureAwait(false);

                        compiledTs = true;
                    }

                    var compiledName = file.Name.Replace(".ts", ".js");

                    var outputPath = basePath + compiledName;

                    var fileStream = File.Open(outputPath, FileMode.Open, FileAccess.Read);

                    var blob = new Blob(compiledName, fileStream, new BlobMetadata {
                        ContentType = "application/javascript"
                    });

                    log.Info($"Compiled '{compiledName}'");

                    await bucket.PutAsync(blob).ConfigureAwait(false);
                }
                else if (FormatHelper.IsStaticFormat(format))
                {
                    var blob = new Blob(
                        key    : file.Name,
                        stream : await file.ToMemoryStreamAsync().ConfigureAwait(false)
                    );
                    
                    // Metadata / ContenType?

                    await bucket.PutAsync(blob).ConfigureAwait(false);
                }
            }

            result.Elapsed = sw.Elapsed;

            return result;
        }

        #region Compilers

        private async Task<Stream> CompileCssAsync(IBlob file)
        {
            var sourceText = await file.ReadAllTextAsync().ConfigureAwait(false);

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

            return output;
        }

        #endregion

        public void Dispose()
        {
            Directory.Delete(basePath, recursive: true);  // Delete the build folder
        }
    }

    internal static class StreamExtensions
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