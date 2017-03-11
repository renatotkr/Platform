using System.Data;

namespace Carbon.Platform
{
    using Data;
    using Json; 

    public class JsonObjectHandler : DbTypeHandler<JsonObject>
    {
        public override DatumInfo DatumType => DatumInfo.String(4000);

        public override JsonObject Parse(object value) => JsonObject.Parse((string)value);

        public override void SetValue(IDbDataParameter parameter, JsonObject value)
        {
            parameter.Value = value.ToString(pretty: false);
            parameter.DbType = DbType.String;
        }
    }
}