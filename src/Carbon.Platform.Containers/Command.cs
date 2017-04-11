using System.Collections.Generic;
using System.Text;

namespace Carbon.Docker
{
    public struct Command
    {
        private Command(CommandType type, string text)
        {
            Type = type;
            Text = text;
        }

        public CommandType Type { get; set; }

        public string Text { get; }

        public override string ToString()
        {
            return Type.ToString() + " " + Text;
        }

        // Entry point


        // EXPOSE <port> [<port>...]
        // -P         : Publish all exposed ports to the host interfaces
        // -p=[]      : Publish a container᾿s port or a range of ports to the host
        // e.g. -p 1234-1236:1234-1236/tcp

        // More functionality here...

        // Link to other containers...
        // Expose to host

        public static Command ENV(string name, string value)
        {
            return new Command(CommandType.ENV, name + "=" + value);
        }

        public static Command ARG(string name, string value)
        {
            return new Command(CommandType.ARG, name + "=" + value);
        }

        public static Command Stopsignal(int signalNumber)
        {
            return new Command(CommandType.STOPSIGNAL, signalNumber.ToString());
        }

        // LABEL version="1.0"
        public static Command Label(string name, string value)
        {
            return new Command(CommandType.LABEL, name + "=" + "\"" + value + "\"");
        }

        public static Command Env(IEnumerable<KeyValuePair<string, string>> vars)
        {
            // abc=bye def=$abc

            var sb = new StringBuilder();

            var i = 0;

            foreach (var var in vars)
            {
                if (i > 0) sb.Append(" ");

                sb.Append(var.Key + "=" + var.Value);

                i++;
            }

            return new Command(CommandType.ENV, sb.ToString());
        }

        public static Command Add(string source, string destination)
        {
            return Create(CommandType.ADD, source + " " + destination);
        }

        // ADD hom?.txt /mydir/ 
        public static Command Add(string[] volumes)
        {
            return Create(CommandType.VOLUME, FormatParamaters(volumes));
        }

        public static Command Expose(params int[] ports)
        {
            return Create(CommandType.EXPOSE, string.Join(" ", ports));
        }

        public static Command Expose(int start, int end)
        {
            return Create(CommandType.EXPOSE, start + "-" + end);
        }

        public static Command EntryPoint(string[] args)
        {
            return Create(CommandType.ENTRYPOINT, args);
        }

        public static Command Label(string text)
        {
            return new Command(CommandType.LABEL, text);
        }

        public static Command Cmd(params string[] paramaters)
        {
            return Create(CommandType.CMD, paramaters);
        }

        public static Command Run(params string[] paramaters)
        {
            return Create(CommandType.RUN, paramaters);
        }

        public static Command Create(CommandType type, params object[] paramaters)
        {
            return new Command(type, paramaters.Length == 1
                ? paramaters[0].ToString()
                : FormatParamaters(paramaters));
        }

        private static string FormatParamaters(params object[] paramaters)
        {
            var sb = new StringBuilder();

            sb.Append("[");

            int i = 0;

            foreach (var p in paramaters)
            {
                if (i != 0) sb.Append(",");

                if (p is string)
                {
                    sb.Append("\"");
                }

                sb.Append(p);

                if (p is string)
                {
                    sb.Append("\"");
                }

                i++;
            }

            sb.Append("]");

            return sb.ToString();
        }
    }
}

// # Comment
// INSTRUCTION arguments
// The instruction is not case-sensitive. 
// However, convention is for them to be UPPERCASE to distinguish them from arguments more easily.

// https://github.com/dotnet/dotnet-docker
