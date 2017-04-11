using System;
using System.IO;

using Carbon.Css;
using Carbon.Storage;

namespace Carbon.Building.Web
{
    public class CssResolver : ICssResolver
    {
        private static readonly char[] ForwardSlash = { '/' };

        private readonly IPackage package;

        public CssResolver(string scopedPath, IPackage package)
        {
            ScopedPath = scopedPath;

            this.package = package ?? throw new ArgumentNullException(nameof(package));
        }

        // Make OpenAsync...

        public Stream Open(string absolutePath)
        {
            if (absolutePath.StartsWith("/"))
            {
                absolutePath = absolutePath.TrimStart(ForwardSlash);
            }

            return Find(package, absolutePath)?.OpenAsync().Result;
        }

        public string ScopedPath { get; }

        #region Helpers

        public static IBlob Find(IPackage package, string absolutePath)
        {
            foreach (var file in package)
            {
                if (file.Name == absolutePath)
                {
                    return file;
                }
            }

            return null;
        }

        #endregion
    }
}
