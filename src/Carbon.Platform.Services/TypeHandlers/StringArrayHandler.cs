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

        // use JSON when supported by Amazon Auroa...
        
        public override void SetValue(IDbDataParameter parameter, string[] value)
        {
            parameter.Value = string.Join(",", value);
            parameter.DbType = DbType.String;
        }
    }
}