using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using TypeScript;

namespace Carbon.Building.Web
{
    using Storage;

    public class TypeScriptProject
    {
        private readonly IPackage package;
        private readonly TypeScriptCompiler compiler;

        private static readonly SemaphoreSlim gate = new SemaphoreSlim(1, 1);

        private bool built = false;

        public TypeScriptProject(IPackage package, string projectPath)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            ProjectPath = projectPath ?? throw new ArgumentNullException(nameof(projectPath));

            this.compiler = new TypeScriptCompiler(projectPath);
        }

        // // e.g. c:/builds/100/
        public string ProjectPath { get; }

        public async Task BuildAsync(CancellationToken token)
        {
            await gate.WaitAsync(token).ConfigureAwait(false);

            if (built) return;

            try
            {
                await CopyFilesAsync().ConfigureAwait(false);

                await compiler.BuildAsync(token).ConfigureAwait(false);

                built = true;
            }
            finally
            {
                gate.Release();
            }
        }

        #region Helpers

        private async Task CopyFilesAsync()
        {
            foreach (var file in package)
            {
                // including .d.ts

                if (file.Name.EndsWith(".ts") || file.Name == "tsconfig.json")
                {
                    var tempSourcePath = Path.Combine(ProjectPath, file.Name);

                    using (var stream = await file.OpenAsync().ConfigureAwait(false))
                    {
                        // This helper creates the directory if it exists
                        await stream.CopyToFileAsync(tempSourcePath).ConfigureAwait(false);
                    }
                }
            }
        }

        #endregion
    }
}
