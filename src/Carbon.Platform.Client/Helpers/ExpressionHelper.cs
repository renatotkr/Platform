using System.Text;

namespace Carbon.Data.Expressions
{
    public static class ExpressionHelper
    {
        internal static string ToQueryString(this Expression expression)
        {
            if (expression == null) return "";

            var sb = new StringBuilder("?");

            switch (expression)
            {
                case BinaryExpression binary:
                    sb.Append(binary.Left.ToString());
                    sb.Append('=');

                    if (binary.Right is Constant constant)
                    {
                        sb.Append(constant.Value.ToString());
                    }
                    else
                    {
                        sb.Append(binary.Right.ToString());
                    }

                    break;
            }

            return sb.ToString();
        }        
    }
}

// $filter=type eq "app" and ...