using System;

namespace Carbon.Platform.Hosting
{
    public class UpdateDomainRecordRequest
    {
        public UpdateDomainRecordRequest(
            long id, 
            string value)
        {
            #region Preconditions

            if (id <= 0)
                throw new ArgumentException("Must be > 0", nameof(id));

            #endregion

            Id = id;
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public long Id { get; }
        
        public string Value { get; }        
    }
}
