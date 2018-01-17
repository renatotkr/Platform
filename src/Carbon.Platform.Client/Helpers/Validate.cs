using System;

namespace Carbon.Platform
{
    internal static class Validate
    {
        public static void Id(long value, string paramName = "id")
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(paramName, value, "Must be > 0");
            }
        }

        public static void NotNull(object value, string paramName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        public static void NotNullOrEmpty(string value, string paramName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(paramName);
            }

            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Must not be empty", paramName);
            }
        }
    }
}
