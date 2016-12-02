using System;
using System.IO;

namespace Carbon.Builder
{
    using Css;
    using Packaging;

    internal class CssResolver : ICssResolver
    {
        private readonly Package package;

        public CssResolver(string scopedPath, Package package)
        {
            #region Preconditions

            if (package == null) throw new ArgumentNullException(nameof(package));
            
            #endregion

            ScopedPath = scopedPath;

            this.package = package;
        }

        public Stream Open(string absolutePath)
        {
            if (absolutePath.StartsWith("/"))
            {
                absolutePath = absolutePath.TrimStart('/');
            }

            return package.Find(absolutePath)?.Open();
        }

        public string ScopedPath { get; }
    }
}
