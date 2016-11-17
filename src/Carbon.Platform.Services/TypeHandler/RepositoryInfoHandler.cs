using System.Data;

namespace Carbon.Platform
{
    using Data;
    using Data.Annotations;
    using Repositories;

    public class RepositoryInfoHandler : DbTypeHandler<RepositoryInfo>
    {
        public override RepositoryInfo Parse(object value)
            => RepositoryInfo.Parse((string)value);

        public override void SetValue(IDbDataParameter parameter, RepositoryInfo value)
        {
            parameter.Value = value.ToString();
            parameter.DbType = DbType.AnsiString;
        }

        public override DatumInfo DatumType => DatumInfo.String(100);
    }
}
