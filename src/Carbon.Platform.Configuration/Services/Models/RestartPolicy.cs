using System;

namespace Carbon.Platform.Configuration
{
    public class RestartPolicy
    {
        public static readonly RestartPolicy Default = new RestartPolicy(RestartCondition.Always, TimeSpan.FromSeconds(10));

        public RestartPolicy(RestartCondition condition, TimeSpan? delay)
        {
            Condition = condition;
            Delay = delay;
        }

        public RestartCondition Condition { get; }

        // e.g. RestartSec
        public TimeSpan? Delay { get; }
    }

    public enum RestartCondition
    {
        Always = 1
    }
}
