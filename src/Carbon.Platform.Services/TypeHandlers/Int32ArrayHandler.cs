using System.Data;

using Carbon.Extensions;

namespace Carbon.Data
{
    internal class Int32ArrayHandler : DbTypeHandler<int[]>
    {
        public override DatumInfo DatumType => DatumInfo.String(1000);

        public override int[] Parse(object value)
        {
            var text = (string)value;

            var parts = text.Split(Seperators.Comma);

            var result = new int[parts.Length];

            for (var i = 0; i < parts.Length; i++)
            {
                result[i] = int.Parse(parts[i]);
            }

            return result;
        }
        
        public override void SetValue(IDbDataParameter parameter, int[] value)
        {
            // e.g. 1,2,3,4

            parameter.Value = string.Join(",", value);
            parameter.DbType = DbType.String;
        }
    }
}