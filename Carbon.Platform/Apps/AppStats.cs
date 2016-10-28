using Carbon.Data.Annotations;

namespace Carbon.Computing
{
    public class AppStats
    {
        [Member(1)]
        public long AppId { get; set; }

        [Member(2), Mutable]
        public long RequestCount { get; set; }

        // DataIn

        // DataOut
    }
}
