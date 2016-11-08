using System;

namespace Carbon.Computing
{
    using Data.Annotations;

    // Docker container...

    [Dataset("Containers")]
    public class Container : IHost
    {
        [Member(1), Identity]
        public long Id { get; set; }

        [Member(2)]
        public string Name { get; set; }

        [Member(3), Indexed]
        public long HostId { get; set; }

        [Member(4)]
        public DateTime Created { get; set; }
    }
}