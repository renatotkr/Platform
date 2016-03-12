namespace Carbon.Builder
{
    using Carbon.Css;
    using Carbon.Platform;

    internal class CssResolver : ICssResolver
    {
        private readonly string scopedPath;
        private readonly Package package;

        public CssResolver(string scopedPath, Package package)
        {
            this.scopedPath = scopedPath;
            this.package = package;
        }

        public string GetText(string absolutePath)
        {
            absolutePath = absolutePath.TrimStart('/');

            var include = package.Find(absolutePath);

            if (include == null) return null;

            return include.ReadStringAsync().Result;
        }

        public string ScopedPath => scopedPath;
    }
}
