namespace Carbon.Docker
{
    public struct CPUUtilizationPolicy
    {
        // CPU Share (Relative weight)

        public int Share { get; set; }

        public int Period { get; set; }

        public int Quota { get; set; }

        // Memory Mode
    }
}