using System;

namespace Carbon.Building
{
    public class BuildTimeout : Exception
    {
        public BuildTimeout(TimeSpan duration)
            : base($"Build timed out after {duration.TotalSeconds:0} seconds")
        { }
    }
}
