namespace Carbon.Platform.Configuration.Systemd
{
    public class InstallSection : SectionBase
    {
        public string WantedBy
        { 
            get => Get("WantedBy");
            set => Set("WantedBy", value, 1);
        }

        public string RequiredBy
        {
            get => Get("RequiredBy");
            set => Set("RequiredBy", value, 1);
        }

        public string Alias
        {
            get => Get("Alias");
            set => Set("Alias", value, 1);
        }

        public string Also
        {
            get => Get("Also");
            set => Set("Also", value, 1);
        }

        public string DefaultInstance
        {
            get => Get("DefaultInstance");
            set => Set("DefaultInstance", value, 1);
        }
    }
}