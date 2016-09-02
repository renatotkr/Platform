using System;
using System.Collections.Generic;
using System.IO;

namespace Carbon.Packaging
{
    using Storage;

    internal class DirectoryPackage : Package
    {
        private readonly DirectoryInfo root;

        public DirectoryPackage(DirectoryInfo root)
        {
            #region Preconditions

            if (root == null) throw new ArgumentNullException(nameof(root));

            #endregion

            this.root = root;
        }

        public override IEnumerable<IBlob> Enumerate()
        {
            foreach (var file in GetFilesRecursive(root))
            {
                var key = GetKey(file);

                yield return new VolumeFile(key, file);
            }
        }

        #region File Helpers

        //  // Strip off the root and replace \ with /
        public string GetKey(FileInfo file)
            => file.FullName.Replace(root.FullName, "").Replace(@"\", "/").TrimStart('/');

        internal IEnumerable<FileInfo> GetFilesRecursive(DirectoryInfo rootDirectory)
        {
            foreach (var directory in rootDirectory.EnumerateDirectories())
            {
                if (directory.Name.StartsWith("_"))
                {
                    continue;
                }

                foreach (var file in GetFilesRecursive(directory))
                {
                    if (!file.Name.StartsWith("_"))
                    {
                        yield return file;
                    }
                }
            }

            foreach (var file in rootDirectory.EnumerateFiles())
            {
                if (file.Name.StartsWith("_")) continue;

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