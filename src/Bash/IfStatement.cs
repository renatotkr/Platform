using System.Text;

namespace Bash
{
    using Commands;

    public class IfStatement
    {
        public IfStatement(BashExpression condition, Command[] then, Command[] elseThen = null)
        {
            Condition = condition;
            Then      = then;
            Else      = elseThen;
        }

        public IfStatement(BashExpression condition, Command then)
        {
            Condition = condition;
            Then = new[] { then };
        }

        public BashExpression Condition { get; }

        public Command[] Then { get; }

        public ElIfStatement[] ElseIfs { get; set; }

        public Command[] Else { get; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            // if [ ! $# == 2 ]; then
            sb.Append($"if {Condition}; then");

            WriteBlock(sb, Then);

            if (Else != null)
            {
                sb.Append("else");

                WriteBlock(sb, Else);
            }

            sb.Append("fi");

            return sb.ToString();
        }

        // if [ -n "$2" ]; then APPVERSION=$2; else APPVERSION="latest"; fi

        private static bool WriteBlock(StringBuilder sb, Command[] commands)
        {
            if (commands.Length == 1)
            {
                sb.Append(' ');
                sb.Append(commands[0].ToString());
                sb.Append("; ");

                return false;
            }
            else
            {
                foreach(var command in commands)
                {
                    sb.AppendLine();

                    sb.Append("  ");
                    sb.Append(commands.ToString());
                }

                sb.AppendLine();

                return true;
            }
        }
    }

    public class ElIfStatement
    {
        public ElIfStatement(BashExpression condition, Command then)
        {
            Condition = condition;
            Then = then;
        }

        public BashExpression Condition { get; }

        public Command Then { get; }
    }
}