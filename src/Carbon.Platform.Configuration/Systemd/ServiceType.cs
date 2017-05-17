namespace Carbon.Platform.Configuration.Systemd
{
    public enum ServiceType
    {
        Simple  = 1,
        Forking = 2,
        Oneshot = 3,
        Dbus    = 4,
        Notify  = 5,
        Idle    = 6
    }
}
