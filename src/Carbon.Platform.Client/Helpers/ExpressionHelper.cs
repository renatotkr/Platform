using System.Text;

namespace Carbon.Data.Expressions
{
    internal static class ExpressionHelper
    {
        public static string ToQueryString(this Expression expression)
        {
            if (expression == null) return "";

            var sb = new StringBuilder("?");

            switch (expression)
            {
                case BinaryExpression binary:
                    sb.Append(binary.Left.ToString());
                    sb.Append("=");
                    sb.Append(binary.Right.ToString());
                    break;
            }

            return sb.ToString();
        }
    }
}