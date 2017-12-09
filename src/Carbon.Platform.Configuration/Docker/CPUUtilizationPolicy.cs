namespace Carbon.Platform.Configuration.Docker
{
    public struct CPUUtilizationPolicy
    {
        // CPU Share (Relative weight)

        public int Share;

        public int Period;

        public int Quota;

        // Memory Mode
    }
}