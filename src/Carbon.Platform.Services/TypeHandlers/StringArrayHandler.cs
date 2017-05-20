using System.Data;

using Carbon.Extensions;

namespace Carbon.Data
{
    internal class StringArrayHandler : DbTypeHandler<string[]>
    {
        public override DatumInfo DatumType => DatumInfo.String(255);

        public override string[] Parse(object value)
        {
            var text = (string)value;

            return text.Split(Seperators.Comma);
        }

        // JSON when supported by Amazon Auroa...
        
        // NOTES:
        // - May be a mix of IP4 & IP6 addresses
        
        public override void SetValue(IDbDataParameter parameter, string[] value)
        {
            // e.g. 192.164.8.1,56.345.345.1

            parameter.Value = string.Join(",", value);
            parameter.DbType = DbType.String;
        }
    }
}