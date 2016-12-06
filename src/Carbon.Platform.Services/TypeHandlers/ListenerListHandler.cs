using System.Data;
using System.Collections.Generic;

namespace Carbon.Platform
{
    using Networking;
    using Data;

    public class ListenerListHandler : DbTypeHandler<List<Listener>>
    {
        public override List<Listener> Parse(object value)
        {
            var text = (string)value;

            var ports = text.Split(',');

            var list = new List<Listener>(ports.Length);

            foreach (var p in ports)
            {
                list.Add(Listener.Parse(p));
            }

            return list;
        }

        public override void SetValue(IDbDataParameter parameter, List<Listener> value)
        {
            parameter.Value = string.Join(",", value);

            parameter.DbType = DbType.AnsiString;
        }

        public override DatumInfo DatumType => DatumInfo.String(255);
    }
}
