using System.Data;
using System.Collections.Generic;
using System.Net;

namespace Carbon.Platform
{
    using Data.Annotations;
    using Data;

    public class IPAddressListHandler : DbTypeHandler<List<IPAddress>>
    {
        public override List<IPAddress> Parse(object value)
        {
            var text = (string)value;

            var ips = text.Split(',');

            var list = new List<IPAddress>(ips.Length);

            foreach (var p in ips)
            {
                list.Add(IPAddress.Parse(p));
            }

            return list;
        }

        public override void SetValue(IDbDataParameter parameter, List<IPAddress> value)
        {
            parameter.Value = value.ToString();
            parameter.DbType = DbType.AnsiString;
        }

        public override DatumInfo DatumType => DatumInfo.String(255);
    }
}
