using System.Data;

namespace Carbon.Data
{
    using Versioning;

    internal class SemanticVersionHandler : DbTypeHandler<SemanticVersion>
    {
        public override DatumInfo DatumType => DatumInfo.String(100);

        public override SemanticVersion Parse(object value) => 
            SemanticVersion.Parse((string)value);

        public override void SetValue(IDbDataParameter parameter, SemanticVersion value)
        {
            parameter.Value  = value.ToAlignedString();
            parameter.DbType = DbType.AnsiString;
        }
    }
}