namespace Carbon.Cloud.Logging
{
    public enum EdgeCacheStatus : byte
    {
        Unknown      = 0, // not applicable
        Hit          = 1, // direct hit
        Miss         = 2, // passed through to origin
        Expired      = 3, // expired
        Revalidated  = 4  // revalidated
    }
    
    public static class EdgeCacheStatusHelper
    {
        public static EdgeCacheStatus Parse(string text)
        {
            switch (text)
            {
                case "HIT"          : return EdgeCacheStatus.Hit;
                case "MISS"         : return EdgeCacheStatus.Miss;
                case "EXPIRED"      : return EdgeCacheStatus.Expired;
                case "REVALIDATED"  : return EdgeCacheStatus.Revalidated;
                default             : return EdgeCacheStatus.Unknown;
            }
        }
    }
}