using Carbon.Net.Dns;

namespace Carbon.Platform.Hosting
{
    public class UpdateDomainRecordRequest
    {
        public UpdateDomainRecordRequest(long id, IRecord value)
        {
            Ensure.IsValidId(id);
            Ensure.NotNull(value, nameof(value));

            Id = id;
            Value = value;
        }

        public long Id { get; }

        public IRecord Value { get; }
    }
}