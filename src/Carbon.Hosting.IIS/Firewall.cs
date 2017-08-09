using System;
using System.Diagnostics;

namespace Carbon.Hosting
{
    public sealed class Firewall
    {
        public bool Exists(string name, ushort port)
        {
            var command = $@"advfirewall firewall show rule name=""{name}""";

            var result = ExecNetSh(command);

            /*
            netsh>
            Rule Name:                            appname
            ----------------------------------------------------------------------
            Enabled:                              Yes
            Direction:                            In
            Profiles:                             Domain,Private,Public
            Grouping:
            LocalIP:                              Any
            RemoteIP:                             Any
            Protocol:                             TCP
            LocalPort:                            5000
            RemotePort:                           Any
            Edge traversal:                       No
            Action:                               Allow
            Ok.
            */

            return result.Contains(port.ToString());
        }

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

        private string ExecNetSh(string command)
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

            process.StandardInput.WriteLine(command);
            process.StandardInput.Flush();
            process.StandardInput.Dispose();

            var output = process.StandardOutput.ReadToEnd(); 

            // Console.WriteLine("firewall command result:" + output);

            if (!process.WaitForExit(2000))
            {
                Console.WriteLine("Did not exit in 2s, killed");
                process.Kill();
            }

            process.Dispose();

            return output;
        }
    }
}