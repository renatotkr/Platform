using System.Data;
using System.Collections.Generic;

namespace Carbon.Platform
{
    using Data.Annotations;
    using Computing;
    using Data;

    public class NetworkPortListHandler : DbTypeHandler<List<NetworkPort>>
    {
        public override List<NetworkPort> Parse(object value)
        {
            var text = (string)value;

            var ports = text.Split(',');

            var list = new List<NetworkPort>(ports.Length);

            foreach (var p in ports)
            {
                list.Add(NetworkPort.Parse(p));
            }

            return list;
        }

        public override void SetValue(IDbDataParameter parameter, List<NetworkPort> value)
        {
            parameter.Value = value.ToString();
            parameter.DbType = DbType.AnsiString;
        }

        public override DatumInfo DatumType => DatumInfo.String(100);
    }
}
