using System;

namespace Carbon.Rds
{
    internal static class Ensure
    {
        public static void IsValidId(long value, string name = "id")
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
                throw new ArgumentException("Must not be empty", nameof(name));
            }
        }
    }
}
