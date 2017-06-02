namespace Carbon.Kms
{
    public enum KeyStatus : byte
    {
        Active      = 1,
        Suspended   = 2,
        Deactivated = 3,
        Compromised = 4,
        Destroyed   = 5
    }
}