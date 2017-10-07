using System;

namespace Carbon.Platform.Hosting
{
    public class CreateDomainRecordRequest
    {
        public CreateDomainRecordRequest(
            string name, 
            DomainRecordType type, 
            string value, 
            TimeSpan? ttl)
        {
            #region Preconditions

            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Required", nameof(value));

            if (ttl != null && ttl.Value < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(ttl), ttl.Value.TotalSeconds, "Must be >= 0");

            #endregion

            Name     = name;
            Type     = type;
            Value    = value;
            Ttl      = ttl;
        }

        public string Name { get; }

        public DomainRecordType Type { get; }

        public string Value { get; }
        
        public TimeSpan? Ttl { get; }
    }
}
