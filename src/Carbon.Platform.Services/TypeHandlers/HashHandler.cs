using System.Data;

namespace Carbon.Platform
{
    using Data;
    using Protection;

    public class HashHandler : DbTypeHandler<Hash>
    {
        public override Hash Parse(object value)
            => Hash.Deserialize((byte[])value);

        public override void SetValue(IDbDataParameter parameter, Hash value)
        {
            parameter.Value = value.Serialize();
            parameter.DbType = DbType.Binary;
        }

        public override DatumInfo DatumType => DatumInfo.Binary(65);
    }
}