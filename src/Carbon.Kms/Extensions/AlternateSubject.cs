namespace Carbon.Kms
{
    public static partial class X509Certificate2Extensions
    {
        public readonly struct AlternateSubject
        {
            public AlternateSubject(string type, string name)
            {
                Type = type;
                Name = name;
            }

            public readonly string Type;

            public readonly string Name;

            public void Deconstruct(out string type, out string name)
            {
                type = Type;
                name = Name;
            }
        }
    }

    // https://en.wikipedia.org/wiki/Subject_Alternative_Name

    /*
    public enum SubjectType
    {
        EmailAddress,
        IpAddress,
        URI,
        DNS
    }
    */
}
