namespace Carbon.Platform
{
    using Carbon.Data;

    public static class Initializer
    {
        public static bool IsInitialized = false;

        private static object _sync = new object();

        public static void Run()
        {
            if (IsInitialized) return;

            lock (_sync)
            {
                if (!IsInitialized)
                {
                    ConverterFactory.Add<Semver>(new SemverConverter());

                    IsInitialized = true;
                }
            }
        }

        internal class SemverConverter : IConverter
        {
            public object FromNode(XNode node, ModelMember member)
                => Semver.Parse(node.ToString());

            public XNode ToNode(object value, ModelMember member)
                => new XString(value.ToString());
        }
    }
}
