using System;

namespace Carbon.Platform.Configuration
{
    public class ProgramExecutable
    {
        public ProgramExecutable(string fileName, string arguments = null)
        {
            FileName  = fileName ?? throw new ArgumentException(nameof(fileName));
            Arguments = arguments;
        }

        public string FileName { get; }

        public string Arguments { get; }

        // e.g. Accelerator -port 5000
        public static ProgramExecutable Parse(string text)
        {
            #region Preconditions

            if (text == null)
                throw new ArgumentNullException(nameof(text));

            #endregion

            var parts = text.Split(new[] { ' ' }, 2);

            if (parts.Length == 2)
            {
                return new ProgramExecutable(parts[0], parts[1]);
            }

            return new ProgramExecutable(parts[0]);
        }

        public override string ToString()
        {
            return (Arguments == null)
                ? FileName
                : FileName + " " + Arguments;
        }
    }
}

// https://en.wikipedia.org/wiki/Executable