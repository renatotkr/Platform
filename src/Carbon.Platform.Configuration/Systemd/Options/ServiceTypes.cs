namespace Carbon.Platform.Configuration.Systemd
{
    public static class ServiceTypes
    {
        public const string Simple  = "simple";
        public const string Forking = "forking";
        public const string Oneshot = "oneshot";
        public const string Dbus    = "dbus";
        public const string Notify  = "notify";
        public const string Idle    = "idle";
    }
}