using System.Data;
using System.Text;

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
            parameter.Value  = ToAlignedString(value);
            parameter.DbType = DbType.AnsiString;
        }
        
        private static string ToAlignedString(SemanticVersion version)
        {
            if (version.Major == -1) return "xxxx.xxxx.xxxx";

            var sb = new StringBuilder(14);

            // support 
            // 9999 majors
            // 9999 minors
            // 9999 patches

            sb.Append(version.Major.ToString("0000"));
            sb.Append('.');
            sb.Append(version.Minor == -1 ? "xxxx" : version.Minor.ToString("0000"));
            sb.Append('.');
            sb.Append(version.Patch == -1 ? "xxxx" : version.Patch.ToString("0000"));

            if (version.IsPrerelease)
            {
                sb.Append("-");
                sb.Append(version.Prerelease);
            }

            return sb.ToString();
        }
    }
}

// DB Serialization
/// 0001.0001.0000-prerelease