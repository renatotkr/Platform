using System.Data;

using Carbon.Net;

namespace Carbon.Platform
{
    using Data;

    public class MacAddressHandler : DbTypeHandler<MacAddress>
    {
        public override DatumInfo DatumType => DatumInfo.Binary(6, isFixedSize: true);

        public override MacAddress Parse(object value)
        {
            return new MacAddress((byte[])value);
        }
        
        public override void SetValue(IDbDataParameter parameter, MacAddress value)
        {
            parameter.Value  = value.GetAddressBytes();
            parameter.Size   = 6;
            parameter.DbType = DbType.Binary;
        }
    }
}