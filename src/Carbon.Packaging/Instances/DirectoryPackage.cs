using System;
using System.Collections.Generic;
using System.IO;

using Carbon.Extensions;
using Carbon.Storage;

namespace Carbon.Packaging
{
    internal class DirectoryPackage : Package
    {
        private readonly DirectoryInfo root;

        public DirectoryPackage(DirectoryInfo root)
        {
            this.root = root ?? throw new ArgumentNullException(nameof(root));
        }

        public override IEnumerable<IBlob> Enumerate()
        {
            foreach (var file in GetFilesRecursive(root))
            {
                var key = GetKey(file);

                yield return new PhysicalFileBlob(key, file);
            }
        }

        #region File Helpers

        //  // Strip off the root and replace \ with /
        private string GetKey(FileInfo file)
        {
            return file.FullName
                .Replace(root.FullName, string.Empty)
                .Replace('\\', '/')
                .TrimStart(Seperators.ForwardSlash);
        }

        internal IEnumerable<FileInfo> GetFilesRecursive(DirectoryInfo rootDirectory)
        {
            foreach (var directory in rootDirectory.EnumerateDirectories())
            {
                if (directory.Name[0] == '_')
                {
                    continue;
                }

                foreach (var file in GetFilesRecursive(directory))
                {
                    if (file.Name[0] == '_') continue;

                    yield return file;
                }
            }

            foreach (var file in rootDirectory.EnumerateFiles())
            {
                if (file.Name[0] == '_') continue;

                yield return file;
            }
        }

        #endregion

        public override void Dispose()
        {
            // Cleanup
        }
    }
}