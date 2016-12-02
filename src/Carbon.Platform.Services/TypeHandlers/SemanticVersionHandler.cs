using System.Data;

namespace Carbon.Platform
{
    using Data;
    using Versioning;

    public class SemanticVersionHandler : DbTypeHandler<SemanticVersion>
    {
        public override SemanticVersion Parse(object value)
        {
            return SemanticVersion.Parse((string)value);
        }

        public override void SetValue(IDbDataParameter parameter, SemanticVersion value)
        {
            parameter.Value = value.ToAlignedString();
            parameter.DbType = DbType.AnsiString;
        }

        public override DatumInfo DatumType => DatumInfo.String(100);
    }
}