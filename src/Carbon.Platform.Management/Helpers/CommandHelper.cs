using System;
using System.Linq;

namespace Carbon.Platform.Management
{
    internal static class CommandHelper
    {
        public static string[] ToLines(string text)
        {
            return text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Where(line => !line.StartsWith("#"))
                .ToArray();
        }
    }
}
