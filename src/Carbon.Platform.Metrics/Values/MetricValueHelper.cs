using System;

namespace Carbon.Platform.Metrics
{
    internal static class MetricValue
    {
        public static object Parse(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (text[text.Length - 1] == 'i')
            {
                return long.Parse(text.Substring(0, text.Length - 1));
            }
            else if (text[0] == '"')
            {
                return text.Trim('"');
            }
            else if (text[0] == '-' || char.IsDigit(text[0]))
            {
                return double.Parse(text);
            }
           
            switch (text)
            {
                case "t":
                case "true"  : return true;
                case "f":
                case "false" : return false;
            }

            throw new Exception("Unexpected metric value:" + text);
        }

        public static string Format(object value)
        {
            switch (value)
            {
                case double f : return f.ToString();
                case long i   : return i.ToString() + "i";
                case bool b   : return b ? "true" : "false";
                case string s : return "\"" + value.ToString() + "\"";
            }

            throw new Exception("Invalid data type:" + value.GetType().Name);
        }
    }
}