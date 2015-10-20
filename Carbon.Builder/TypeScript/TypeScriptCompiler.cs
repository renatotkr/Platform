namespace TypeScript
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using Carbon.Platform;

    public class TypeScriptCompiler : IBuilder
    {
        // Global lock
        private static AsyncLock semaphore = new AsyncLock();

        public static string WorkingDirectory = @"D:\tsc\";

        private readonly TypeScriptCompileOptions options;

        public TypeScriptCompiler(string projectPath)
            : this(new TypeScriptCompileOptions(projectPath: projectPath))
        { }

        public TypeScriptCompiler(TypeScriptCompileOptions options)
        {
            #region Preconditions

            if (options == null) throw new ArgumentNullException(nameof(options));

            #endregion

            this.options = options;
        }

        public async Task<BuildResult> BuildAsync()
        {
            using (await semaphore.LockAsync().ConfigureAwait(false))
            {
                return Execute();
            }
        }

        public Task<IDisposable> LockAsync(TimeSpan timeout)
        {
            return semaphore.LockAsync(timeout);
        }

        private BuildResult Execute()
        {
            var command = "tsc ";

            command += options.ToString();

            var result = new BuildResult();

            var timeout = TimeSpan.FromSeconds(10);

            var psi = new ProcessStartInfo(WorkingDirectory + "node", command) {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                WorkingDirectory = WorkingDirectory
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
                        if (error.Length > 0) throw new Exception(error.ToString());

                        var text = output.ToString();

                        string line;

                        using (var reader = new StringReader(text))
                        {
                            while ((line = reader.ReadLine()) != null)
                            {
                                result.Diagnostics.Add(TypeScriptDiagonstic.Parse(line));
                            }
                        }

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