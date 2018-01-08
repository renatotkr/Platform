using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Carbon.Building;
using Carbon.Diagnostics;

namespace TypeScript
{

    public class TypeScriptCompiler : IBuilder
    {
        private static readonly SemaphoreSlim _gate = new SemaphoreSlim(1, 1);

        // TODO: Change back to D

        public static string NodeExecutable   = @"D:\\tsc\\node";
        public static string WorkingDirectory = @"D:\\tsc\\";
        public static string TscScriptPath    = "tsc";

        private readonly CompilerOptions options;
        
        public TypeScriptCompiler(string projectPath)
            : this(new CompilerOptions(projectPath))
        { }

        public TypeScriptCompiler(CompilerOptions options)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task<BuildResult> BuildAsync(CancellationToken ct = default)
        {
            // var sw = Stopwatch.StartNew();

            await _gate.WaitAsync(ct).ConfigureAwait(false);

            // sw.Stop();

            // var waitTime = sw.Elapsed;
              
            try
            {
                return Execute();
            }
            finally
            {
                _gate.Release();
            }
        }

        public static SemaphoreSlim GlobalLock => _gate;

        private BuildResult Execute()
        {
            var sw = Stopwatch.StartNew();

            var command = TscScriptPath + " " + options.ToString();
            
            var timeout = TimeSpan.FromSeconds(15);

            var psi = new ProcessStartInfo(NodeExecutable, command) {
                CreateNoWindow          = true,
                UseShellExecute         = false,
                RedirectStandardError   = true,
                RedirectStandardOutput  = true,
                WorkingDirectory        = WorkingDirectory
            };

            var output = new StringBuilder();
            var error = new StringBuilder();
            var artifacts = new List<Artifact>();
            var diagnostics = new DiagnosticList();

            void Watcher_Changed(object sender, FileSystemEventArgs e)
            {
                if (artifacts.Any(r => r.Path == e.FullPath)) return;

                if (e.ChangeType == WatcherChangeTypes.Created
                    || e.ChangeType == WatcherChangeTypes.Changed)
                {
                    artifacts.Add(new Artifact(e.FullPath));
                }
            }

            using (var watcher = new FileSystemWatcher(options.ProjectPath) {
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName })
            using (var process = new Process { StartInfo = psi })
            {
                watcher.Changed += Watcher_Changed;

                watcher.IncludeSubdirectories = true;
                watcher.EnableRaisingEvents = true;
                
                process.Start();

                using (var outputWaitHandle = new AutoResetEvent(false))
                using (var errorWaitHandle = new AutoResetEvent(false))
                {
                    process.OutputDataReceived += (sender, e) =>
                    {
                        if (e.Data == null)
                        {
                            outputWaitHandle.Set();
                        }
                        else
                        {
                            output.AppendLine(e.Data);
                        }
                    };

                    process.ErrorDataReceived += (sender, e) =>
                    {
                        if (e.Data == null)
                        {
                            errorWaitHandle.Set();
                        }
                        else
                        {
                            error.AppendLine(e.Data);
                        }
                    };

                    process.Start();

                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    if (process.WaitForExit((int)timeout.TotalMilliseconds) &&
                        outputWaitHandle.WaitOne(timeout) &&
                        errorWaitHandle.WaitOne(timeout))
                    {
                        // if there were errors, throw an exception
                        if (error.Length > 0)
                        {
                            throw new Exception(error.ToString());
                        }

                        var text = output.ToString();

                        string line;

                        using (var reader = new StringReader(text))
                        {
                            while ((line = reader.ReadLine()) != null)
                            {
                                diagnostics.Add(TypeScriptDiagonstic.Parse(line));
                            }
                        }

                        return new BuildResult(
                            status      : BuildStatus.Completed, 
                            elapsed     : sw.Elapsed, 
                            artifacts   : artifacts,
                            diagnostics : diagnostics
                        );
                    }
                    else
                    {
                        throw new BuildTimeout(timeout);
                    }
                };
            }
        }
    }
}