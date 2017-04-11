using System.Runtime.Serialization;

namespace Carbon.Platform
{
    [DataContract]
    public class ProcessorStats
    {
        /// <summary>
        /// Amount of time the CPU was busy executing code in user space.
        /// </summary>
        [DataMember(Order = 1, EmitDefaultValue = false)]
        public float UserTime { get; set; }

        /// <summary>
        /// Amount of time the CPU was busy executing code in kernel space. 
        /// </summary>
        [DataMember(Order = 2, EmitDefaultValue = false)]
        public float SystemTime { get; set; }

        /// <summary>
        /// The amount of time the operating system wanted to execute, but was not allowed to by the hypervisor.
        /// </summary>
        [DataMember(Order = 3, EmitDefaultValue = false)]
        public float StealTime { get; set; }
    }
}

// https://en.wikipedia.org/wiki/CPU_time

    
