using System.Data;

namespace Carbon.Platform
{
    using Computing;
    using Data;
    using Json;

    public class HealthCheckHandler : DbTypeHandler<HealthCheck>
    {
        public override DatumInfo DatumType => DatumInfo.String(1000);

        public override HealthCheck Parse(object value)
        {
            return JsonObject.Parse((string)value).As<HealthCheck>();
        }

        public override void SetValue(IDbDataParameter parameter, HealthCheck value)
        {
            parameter.Value = JsonObject.FromObject(value).ToString(pretty: false);
            parameter.DbType = DbType.String;
        }
    }
}