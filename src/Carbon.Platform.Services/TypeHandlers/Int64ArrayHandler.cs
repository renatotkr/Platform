using System.Data;

namespace Carbon.Platform
{
    using Data;

    public class Int64ArrayHandler : DbTypeHandler<long[]>
    {
        public override DatumInfo DatumType => DatumInfo.String(1000);

        public override long[] Parse(object value)
        {
            var text = (string)value;

            var parts = text.Split(',');

            var result = new long[parts.Length];

            for (var i = 0; i < parts.Length; i++)
            {
                result[i] = long.Parse(parts[i]);
            }

            return result;
        }
        
        public override void SetValue(IDbDataParameter parameter, long[] value)
        {
            // e.g. 1,2,3,4

            parameter.Value = string.Join(",", value);
            parameter.DbType = DbType.String;
        }
    }
}