using System;

using Carbon.Net.Dns;

namespace Carbon.Platform.Hosting
{
    public class UpdateDomainRecordRequest
    {
        public UpdateDomainRecordRequest(long id, IRecord value)
        {
            #region Preconditions

            if (id <= 0)
                throw new ArgumentException("Must be > 0", nameof(id));

            #endregion

            Id = id;
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public long Id { get; } // RecordId
        
        public IRecord Value { get; } 
    }
}