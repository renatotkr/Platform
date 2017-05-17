using System;

namespace Carbon.Platform.Configuration
{
    public class ProgramRestartPolicy
    {
        public static readonly ProgramRestartPolicy Default = new ProgramRestartPolicy(
            condition : RestartCondition.Always, 
            delay     : TimeSpan.FromSeconds(10)
        );

        public ProgramRestartPolicy(RestartCondition condition, TimeSpan? delay)
        {
            Condition = condition;
            Delay = delay;
        }

        public RestartCondition Condition { get; }

        // maps to Systemd.Service.RestartSec
        public TimeSpan? Delay { get; }
    }

    public enum RestartCondition
    {
        None      = 0,

        // Clean exits, Unclean exits, and timeouts
        Always    = 1,

        // Unclean exists
        OnFailure = 2,

        // Failure or timeouts
        OnAbormal = 3
    }
}
