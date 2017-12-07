using System;

using Carbon.Net.Dns;

namespace Carbon.Platform.Hosting
{
    public class UpdateDomainRecordRequest
    {
        public UpdateDomainRecordRequest(long id, IRecord value)
        {
            Validate.Id(id);

            Id = id;
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public long Id { get; }

        public IRecord Value { get; }
    }
}