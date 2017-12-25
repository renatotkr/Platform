using System.Collections.Generic;
using System.IO;

namespace Carbon.Platform.Management
{
    internal static class CommandHelper
    {
        public static string[] ToLines(string text)
        {
            Validate.NotNullOrEmpty(text, nameof(text));

            string line;
            var lines = new List<string>();

            using (var reader = new StringReader(text))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    // skip empty lines and comments
                    if (line.Length == 0 || line[0] == '#') continue;

                    lines.Add(line);
                }
            }

            return lines.ToArray();
        }
    }
}