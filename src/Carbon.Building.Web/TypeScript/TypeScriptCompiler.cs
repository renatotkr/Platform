using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Carbon.Building;

namespace TypeScript
{
    public class TypeScriptCompiler : IBuilder
    {
        private static readonly SemaphoreSlim _gate = new SemaphoreSlim(1, 1);

        public static string WorkingDirectory = @"D:\tsc\";

        private readonly CompilerOptions options;

        public TypeScriptCompiler(string projectPath)
            : this(new CompilerOptions(projectPath))
        { }

        public TypeScriptCompiler(CompilerOptions options)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task<BuildResult> BuildAsync(CancellationToken ct)
        {
            var sw = Stopwatch.StartNew();

            await _gate.WaitAsync(ct).ConfigureAwait(false);

            sw.Stop();

            var waitTime = sw.Elapsed;

            BuildResult result;
              
            try
            {
                result = Execute();

                result.WaitTime = waitTime;
            }
            finally
            {
                _gate.Release();
            }

            return result;
        }

        public static SemaphoreSlim GlobalLock => _gate;

        private BuildResult Execute()
        {
            var sw = Stopwatch.StartNew();

            var command = "tsc ";

            command += options.ToString();

            var result = new BuildResult();

            var timeout = TimeSpan.FromSeconds(15);

            var psi = new ProcessStartInfo(WorkingDirectory + "node", command) {
                CreateNoWindow          = true,
                UseShellExecute         = false,
                RedirectStandardError   = true,
                RedirectStandardOutput  = true,
                WorkingDirectory        = WorkingDirectory
            };

            var output = new StringBuilder();
            var error = new StringBuilder();

            using (var process = new Process { StartInfo = psi })
            {
                process.Start();

                using (var outputWaitHandle = new AutoResetEvent(false))
                using (var errorWaitHandle = new AutoResetEvent(false))
                {
                    process.OutputDataReceived += (sender, e) => {
                        if (e.Data == null)
                        {
                            outputWaitHandle.Set();
                        }
                        else
                        {
                            output.AppendLine(e.Data);
                        }
                    };

                    process.ErrorDataReceived += (sender, e) => {
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
                                result.Diagnostics.Add(TypeScriptDiagonstic.Parse(line));
                            }
                        }

                        result.Elapsed = sw.Elapsed;

                        return result;
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