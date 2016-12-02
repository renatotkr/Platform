using System.Data;
using System.Collections.Generic;
using System.Net;

namespace Carbon.Platform
{
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

        // JSON when supported by Amazon Auroa...
        
        // NOTES:
        // - May be a mix of IP4 & IP6 addresses
        
        public override void SetValue(IDbDataParameter parameter, List<IPAddress> value)
        {
            // e.g. 192.164.8.1,56.345.345.1

            parameter.Value = string.Join(",", value);
            parameter.DbType = DbType.AnsiString;
        }

        public override DatumInfo DatumType => DatumInfo.String(255);
    }
}