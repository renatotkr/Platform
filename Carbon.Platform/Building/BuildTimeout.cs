using System;

namespace Carbon.Platform
{
    public class BuildTimeout : Exception
    {
        public BuildTimeout(TimeSpan duration)
            : base($"Build timed out after {duration.TotalSeconds:0} seconds")
        { }
    }
}
