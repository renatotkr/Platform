using System;

namespace Bash
{
    public enum BracketOption
    {
        Single = 1,
        Double = 2
    }

    public class BashExpression
    {
        public BashExpression(string text, BracketOption brackets = BracketOption.Single)
        {
            Text = text ?? throw new Exception(nameof(text));
            Brackets = brackets;    
        }

        public string Text { get; }

        public BracketOption Brackets { get; }

        public static BashExpression IsDefined(string text)
        {
            // -n = non-null || zero length

            return new BashExpression($"-n {text}");
        }

        // -z STRING
 

        public static BashExpression IsNullOrUnset(string text)
        {
            // z= null or unset
            return new BashExpression($"-z {text}");
        }

        public static BashExpression Gt(string variableName, string value)
        {
            return new BashExpression($"{variableName} -gt {value}");
        }

        public static BashExpression Lt(string variableName, string value)
        {
            return new BashExpression($"{variableName} -lt {value}");
        }

        #region Files


        // -d FILE (file exists + is directory)
        // -e FILE (file exists)
        // -r FILE (file exists and is readable)
        // -s FILE (file exists and not empty)
        // -w FILE (file exists and is writable)
        // -x FILE (file exists and is exectuable)

        public static BashExpression DirectoryExists(string file)
        {
            return new BashExpression($"-d {file}");
        }

        public static BashExpression FileExists(string file)
        {
            return new BashExpression($"-e {file}");
        }

        public static BashExpression IsReadable(string file)
        {
            return new BashExpression($"-r {file}");
        }

        public static BashExpression IsNotEmpty(string file)
        {
            return new BashExpression($"-s {file}");
        }

        public static BashExpression IsWritable(string file)
        {
            return new BashExpression($"-w {file}");
        }

        public static BashExpression IsExecutable(string file)
        {
            return new BashExpression($"-x {file}");
        }

        #endregion

        public override string ToString()
        {
            if (Brackets == BracketOption.Double)
            {
                return "[[" + Text + "]]";
            }
            else
            {
                return "[" + Text + "]";
            }
        }
    }

    public class BinaryExpression
    {
        public BinaryExpression(BashExpression lhs, BashExpression rhs, BinaryExpressionType type)
        {
            Left  = lhs;
            Right = rhs;
            Type  = type;
        }

        public BashExpression Left { get; }

        public BashExpression Right { get; }

        public BinaryExpressionType Type { get; }

        public override string ToString()
        {
            var op = Type == BinaryExpressionType.And ? "&&" : "||";

            return Left.ToString() + " " + op + " " + Right.ToString();
        }

        // [ -r $1 ] && [ -s $1 ]

        public static BinaryExpression And(BashExpression lhs, BashExpression rhs)
        {
            return new BinaryExpression(lhs, rhs, BinaryExpressionType.And);
        }

        public static BinaryExpression Or(BashExpression lhs, BashExpression rhs)
        {
            return new BinaryExpression(lhs, rhs, BinaryExpressionType.Or);
        }

        public enum BinaryExpressionType
        {
            And,
            Or
        }
    }

}

// http://tldp.org/LDP/Bash-Beginners-Guide/html/sect_07_01.html
