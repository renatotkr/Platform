using System;
using System.Diagnostics;

namespace Carbon.Hosting.IIS
{
    public class Firewall
    {
        public void Open(string name, ushort port)
        {
            var command = $@"advfirewall firewall add rule name=""{name}"" dir=in action=allow localport={port} protocol=tcp";
            
            ExecNetSh(command);
        }

        public void Close(string name)
        {
            var command = $@"advfirewall firewall delete rule name=""{name}""";

            ExecNetSh(command);
        }

        private void ExecNetSh(string command)
        {
            var process = new Process {
                StartInfo = new ProcessStartInfo("netsh") { 
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true
                }
            };

            process.Start();

            // Console.WriteLine("command:" + command);
            
            // exec 
            process.StandardInput.WriteLine(command);
            process.StandardInput.Flush();
            process.StandardInput.Dispose();

            // To avoid deadlocks, always read the output stream first and then wait.
            var output = process.StandardOutput.ReadToEnd();

            Console.WriteLine("output:" + output);

            if (!process.WaitForExit(2000))
            {
                Console.WriteLine("Did not exit in 2s, killed");
                process.Kill();
            }

            // Console.WriteLine("ok");

            process.Dispose();
        }

        // var process = Process.Start("netsh", "exec " + script);
    }
}