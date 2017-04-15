namespace Carbon.Platform.Sequences
{
    public struct Range
    {
        public Range(long start, long end)
        {
            Start = start;
            End = end;
        }

        public long Start { get; }

        public long End { get; }
    }
}
