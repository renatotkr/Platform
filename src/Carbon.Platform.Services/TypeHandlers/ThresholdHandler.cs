using System.Data;

namespace Carbon.Platform
{
    using Data;
    using Carbon.Platform.Computing;

    public class ThresholdHandler : DbTypeHandler<Threshold>
    {
        public override DatumInfo DatumType => DatumInfo.String(30, isFixedSize: false);

        public override Threshold Parse(object value) => Threshold.Parse((string)value);

        public override void SetValue(IDbDataParameter parameter, Threshold value)
        {
            parameter.Value = value.ToString();
            parameter.DbType = DbType.String;
        }
    }
}