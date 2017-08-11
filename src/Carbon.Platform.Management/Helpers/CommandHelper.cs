using System;
using System.Linq;

namespace Carbon.Platform.Management
{
    internal static class CommandHelper
    {
        public static string[] ToLines(string text)
        {
            #region Preconditions

            if (text == null)
                throw new ArgumentNullException(nameof(text));

            #endregion

            return text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Where(line => !line.StartsWith("#"))
                .ToArray();
        }
    }
}
