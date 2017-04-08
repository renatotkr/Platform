using System.Data;

namespace Carbon.Platform
{
    using Diagnostics.Identity;
    using Data;

    public class BigIdHandler : DbTypeHandler<BigId>
    {
        public override DatumInfo DatumType => DatumInfo.Binary(16, isFixedSize: true);

        public override BigId Parse(object value)
        {
            return BigId.Deserialize((byte[])value);
        }
        
        public override void SetValue(IDbDataParameter parameter, BigId value)
        {
            parameter.Value  = value.Serialize();
            parameter.Size   = 16;
            parameter.DbType = DbType.Binary;
        }
    }
}