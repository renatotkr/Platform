namespace TypeScript
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public class TypeScriptCompiler
    {
        private AsyncLock semaphore = new AsyncLock();

        public static string WorkingDirectory = @"D:\tsc\";

        public Task<IDisposable> LockAsync(TimeSpan timeout)
        {
            return semaphore.LockAsync(timeout);
        }

        public async Task<CompileResult> BuildAsync(string tsPath)
        {
            using (await semaphore.LockAsync().ConfigureAwait(false))
            {
                return Compile(null,
                    new TypeScriptCompileOptions {
                        ProjectPath = tsPath
                    }
                );
            }
        }

        public CompileResult Compile(string tsPath, TypeScriptCompileOptions options)
        {
            var command = "tsc ";

            if (tsPath != null)
            {
                command += tsPath + " ";
            }

            command += options.ToString();

            var timeout = TimeSpan.FromSeconds(10);

            var psi = new ProcessStartInfo(WorkingDirectory + "node", command)
            {
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

                        return new CompileResult(output.ToString()) {
                            Command = command
                        };
                    }
                    else
                    {
                        throw new Exception("Compile operation timed out.");
                    }
                };
            }
        }
    }

    public class TypeScriptCompileOptions
    {
        public enum Version
        {
            ES3,
            ES5,
            ES6
        }

        public string ProjectPath { get; set; }

        public bool EmitComments { get; set; }

        public bool GenerateDeclaration { get; set; }

        public bool GenerateSourceMaps { get; set; }

        public string OutPath { get; set; }

        public Version? TargetVersion { get; set; }

        public override string ToString()
        {
            var d = new Dictionary<string, string>();


            // By invoking tsc with no input files and a - project(or just - p) command line option
            // that specifies the path of a directory containing a tsconfig.json file.

            if (ProjectPath != null)
            {
                d.Add("-p", ProjectPath);
            }

            if (EmitComments == false)
                d.Add("--removeComments", null);

            if (GenerateDeclaration)
                d.Add("-d", null);

            if (GenerateSourceMaps)
                d.Add("--sourcemap", null);

            if (!string.IsNullOrEmpty(OutPath))
                d.Add("--out", OutPath);

            if (TargetVersion != null)
            {
                d.Add("--target", TargetVersion.Value.ToString());
            }

            return string.Join(" ", d.Select(o => o.Key + " " + o.Value));
        }

        // https://github.com/Microsoft/TypeScript/wiki/Compiler-Options
    }

    public class CompileResult
    {
        public CompileResult(string text)
        {
            OutputText = text;
        }

        public string OutputText { get; }

        public string Command { get; set; }
    }
}