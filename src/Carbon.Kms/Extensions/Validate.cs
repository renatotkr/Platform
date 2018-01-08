using System;

namespace Carbon
{
    internal static class Validate
    {
        public static void Id(long value, string name = "id")
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(name, value, "Must be > 0");
            }
        }

        public static void NotNull(object value, string name)
        {
            if (value == null)
            {
                throw new ArgumentNullException(name);
            }
        }

        public static void NotNullOrEmpty(string value, string name)
        {
            if (value == null)
            {
                throw new ArgumentNullException(name);
            }

            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("May not be empty", nameof(name));
            }
        }

        public static void NotNullOrEmpty<T>(T[] value, string name)
        {
            if (value == null)
            {
                throw new ArgumentNullException(name);
            }

            if (value.Length == 0)
            {
                throw new ArgumentException("May not be empty", nameof(name));
            }
        }
    }
}