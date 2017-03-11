using System.Data;

namespace Carbon.Platform
{
    using Data;
    using Networking;

    public class ListenerHandler : DbTypeHandler<Listener>
    {
        public override DatumInfo DatumType => DatumInfo.String(100);

        public override Listener Parse(object value) => Listener.Parse((string)value);

        public override void SetValue(IDbDataParameter parameter, Listener value)
        {
            parameter.Value = value.ToString();
            parameter.DbType = DbType.AnsiString;
        }
    }
}