// Environments span computing & storage

namespace Carbon.Platform
{
    public enum EnvironmentType : byte
    {
        Production   = 1,
        Staging      = 2,
        Intergration = 3,
        Development  = 4
    }

    public static class EnvironmentTypeExtensions
    {
        public static string ToLower(this EnvironmentType type)
        {
            switch (type)
            {
                case EnvironmentType.Production   : return "production";
                case EnvironmentType.Staging      : return "staging";
                case EnvironmentType.Intergration : return "intergration";
                case EnvironmentType.Development  : return "development";
            }

            throw new System.Exception("unexcepted environment type:" + type.ToString());
        }
    }
}