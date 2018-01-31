using System;

namespace Carbon
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

        public static void NotNullOrEmpty<T>(T[] value, string paramName)
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