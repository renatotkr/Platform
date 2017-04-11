using System.Data;

namespace Carbon.Data
{
    using Protection;

    internal class HashHandler : DbTypeHandler<Hash>
    {
        public override DatumInfo DatumType => DatumInfo.Binary(65, isFixedSize: false);

        public override Hash Parse(object value) => Hash.Deserialize((byte[])value);

        public override void SetValue(IDbDataParameter parameter, Hash value)
        {
            parameter.Value = value.Serialize();
            parameter.DbType = DbType.Binary;
        }
    }
}