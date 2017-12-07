using System;
using System.Collections.Generic;
using System.Text;

namespace Carbon.Platform.Configuration.Docker
{
    public class Dockerfile
    {
        public string Name { get; set; }

        public string Tag { get; set; }

        public string Digest { get; set; }

        public string Maintainer { get; set; }

        public List<Command> Commands { get; } = new List<Command>();

        // Ran when machine starts
        // CMD 
        public Command? DefaultCommand { get; set; }

        #region Helpers

        public Dockerfile Run(params string[] command)
        {
            Commands.Add(Command.Run(command));

            return this;
        }

        public Dockerfile Expose(params int[] ports)
        {
            Commands.Add(Command.Expose(ports));

            return this;
        }

        public Dockerfile SetWorkingDirectory(string name)
        {
            Commands.Add(Command.Create(CommandType.WORKDIR, name));

            return this;
        }

        public Dockerfile SetUser(string name)
        {
            Commands.Add(Command.Create(CommandType.USER, name));

            return this;
        }


        public Dockerfile AddVolumes(params string[] parameters)
        {
            // TODO: Find any existing volumne commands -- and update 

            Commands.Add(Command.Create(CommandType.VOLUME, parameters));

            return this;
        }

        public Dockerfile SetEntrypoint(string program, string[] parameters)
        {
            var args = new string[parameters.Length + 1];

            args[0] = program;

            Array.Copy(parameters, 0, args, 1, parameters.Length);

            Commands.Add(Command.EntryPoint(args));

            return this;
        }

        #endregion

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("FROM ");
            sb.Append(Name);

            if (Tag != null)
            {
                sb.Append(':');
                sb.Append(Tag);
            }

            if (Digest != null)
            {
                sb.Append(':');
                sb.Append(Digest);
            }

            sb.AppendLine();

            if (Maintainer != null)
            {
                sb.AppendLine("MAINTAINER " + Maintainer);
            }

            foreach (var command in Commands)
            {
                /*
                RUN<command>(shell form, the command is run in a shell, which by default is /bin/sh -c on Linux or cmd/S /C on Windows)
                RUN["executable", "param1", "param2"](exec form)
                */

                sb.AppendLine(command.ToString());
            }

            // ["executable","param1","param2"]

            if (DefaultCommand != null)
            {
                sb.Append(DefaultCommand.ToString());
            }

            return sb.ToString();
        }
    }
}

// https://docs.docker.com/engine/reference/builder/
// # Comment
// INSTRUCTION arguments
// The instruction is not case-sensitive. 
// However, convention is for them to be UPPERCASE to distinguish them from arguments more easily.

// https://github.com/dotnet/dotnet-docker
