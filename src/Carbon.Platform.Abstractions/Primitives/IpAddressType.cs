namespace Carbon.Net
{
    public enum IPAddressType : byte
    {
        Anycast = 1, // many routes
        Unicast = 2, // 1 route 
        Private = 3
    }
}