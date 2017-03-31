namespace Carbon.Net
{
    public enum NetworkLayer : byte
    {
        Physical     = 1,
        Datalink     = 2, // Link
        Internet     = 3,
        Transport    = 4, // aka end to end
        Session      = 5,
        Presentation = 6,
        Applications = 7
    }
}

// https://tools.ietf.org/html/rfc1122