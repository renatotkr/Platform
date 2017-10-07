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

            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Required", nameof(value));

            #endregion

            Id = id;
            Value = value;
        }

        public long Id { get; }
        
        public string Value { get; }        
    }
}
