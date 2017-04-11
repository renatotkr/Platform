using System;
using System.Data;
using System.Net;

namespace Carbon.Data
{
    internal class IPAddressArrayHandler : DbTypeHandler<IPAddress[]>
    {
        private static readonly char[] comma = { ',' };

        public override DatumInfo DatumType => DatumInfo.String(255);

        public override IPAddress[] Parse(object value)
        {
            var text = (string)value;

            if (text == "") return Array.Empty<IPAddress>();

            var parts = text.Split(comma);

            var list = new IPAddress[parts.Length];

            for(var i = 0; i < parts.Length; i++)
            { 
                list[i] = IPAddress.Parse(parts[i]);
            }

            return list;
        }
        
        // NOTES:
        // - May be a mix of IP4 & IP6 addresses
        
        public override void SetValue(IDbDataParameter parameter, IPAddress[] value)
        {
            // e.g. 192.164.8.1,56.345.345.1
            
            parameter.Value = value.Length == 0 ? "" : string.Join(",", (object[])value);

            parameter.DbType = DbType.AnsiString;
        }
    }
}