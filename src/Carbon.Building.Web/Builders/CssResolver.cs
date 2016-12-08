using System;
using System.IO;

namespace Carbon.Builder
{
    using Css;
    using Storage;

    internal class CssResolver : ICssResolver
    {
        private readonly IPackage package;

        public CssResolver(string scopedPath, IPackage package)
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
