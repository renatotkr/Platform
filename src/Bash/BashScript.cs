using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Bash
{
    using Commands;

    public class BashScript : IEnumerable<Command>
    {
        public readonly List<Command> commands = new List<Command>();

        public void Add(string command)
        {
            commands.Add(new Command(command));
        }

        public void Add(Command command)
        {
            commands.Add(command);
        }

        public void Add(IfStatement command)
        {
            commands.Add(new Command(command.ToString()));
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("#!/bin/bash");

            foreach (var command in commands)
            {
                sb.AppendLine();

                sb.Append(command.ToString());
            }

            return sb.ToString();
        }

        #region IEnumerable

        IEnumerator<Command> IEnumerable<Command>.GetEnumerator() => commands.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => commands.GetEnumerator();

        #endregion
    }
}
