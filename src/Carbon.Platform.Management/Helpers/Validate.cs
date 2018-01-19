using System;

namespace Carbon.Platform
{
    internal static class Ensure
    {
        public static void IsValidId(long value, string paramName = "id")
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

        public static void GreaterThanZero(int value, string paramName)
        {
            if (value <= 0)
            {
                throw new ArgumentException("Must be > 0", paramName);
            }
        }

        public static void NotNullOrEmpty(string value, string paramName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(paramName);
            }

            if (value.Length == 0)
            {
                throw new ArgumentException("May not be empty", paramName);
            }
        }
    }
}
