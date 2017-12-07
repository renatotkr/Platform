namespace Carbon.Platform
{
    public static class InstanceName
    {
        public static string FromDescription(string description)
        {
            if (description == null) return null;

            return description
                .Replace('/', '_')
                .Replace('(', '[')
                .Replace(')', ']')
                .Replace('#', '_');
        }
    }
}