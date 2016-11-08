using System.Data;

namespace Carbon.Platform
{
    using Data.Annotations;
    using Computing;
    using Data;

    public class NetworkPortListHandler : DbTypeHandler<NetworkPortList>
    {
        public override NetworkPortList Parse(object value)
            => NetworkPortList.Parse((string)value);

        public override void SetValue(IDbDataParameter parameter, NetworkPortList value)
        {
            parameter.Value = value.ToString();
            parameter.DbType = DbType.AnsiString;
        }

        public override DatumInfo DatumType => DatumInfo.String(100);
    }
}
