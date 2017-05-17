namespace Carbon.Platform.Computing
{
    public struct ProgramName
    {
        public static bool Validate(string name)
        {
            if (name.Length > 63) return false;

            // Ensure it's alpha numeric
            // What about unicode characters outside of this range?

            foreach (var c in name)
            {
                if (c == '-' || c == '_') continue;

                if (!char.IsLetterOrDigit(c)) return false;
            }

            // <= 63 characters
            // no forbidden characters

            return true;
        }
    }
}


// IIS Restrctions
// length = 64
// forbidden: & / \ : * ? | " < > [ ] + = ; , @

// Lambda Notes
// 64 characters
// ([a-zA-Z0-9-_]+)