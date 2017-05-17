namespace Carbon.Platform.Configuration.Systemd
{
    public class UnitSection : SectionBase
    {
        public string Description
        {
            get => Get("Description");
            set => Set("Description", value, 1);
        }

        public string Documentation
        {
            get => Get("Documentation");
            set => Set("Documentation", value, 2);
        }

        public string Requires
        {
            get => Get("Requires");
            set => Set("Requires", value, 3);
        }

        public string Wants
        {
            get => Get("Wants");
            set => Set("Wants", value, 4);
        }

        public string BindsTo
        {
            get => Get("BindsTo");
            set => Set("BindsTo", value, 5);
        }

        public string Before
        {
            get => Get("Before");
            set => Set("Before", value, 6);
        }

        public string After
        {
            get => Get("After");
            set => Set("After", value, 7);
        }

        public string Conflicts
        {
            get => Get("Conflicts");
            set => Set("Conflicts", value, 8);
        }

        public string Condition
        {
            get => Get("Condition");
            set => Set("Condition", value, 9);
        }

        public string Assert
        {
            get => Get("Assert");
            set => Set("Assert", value, 1);
        }
    }
}
