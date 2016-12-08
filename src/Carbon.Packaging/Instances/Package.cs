using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace Carbon.Packaging
{
    using Storage;

    public abstract class Package : IPackage, IEnumerable<IBlob>, IDisposable
    {
        public abstract IEnumerable<IBlob> Enumerate();

        public abstract void Dispose();

        public IBlob Find(string absolutePath)
        {
            foreach (var file in Enumerate())
            {
                if (file.Name == absolutePath)
                {
                    return file;
                }
            }

            return null;
        }

        public IEnumerable<IBlob> Filter(string prefix)
        {
            foreach (var blob in Enumerate())
            {
                if (blob.Name.StartsWith(prefix))
                {
                    yield return blob;
                }
            }
        }

        #region Helpers

        public static Package FromDirectory(DirectoryInfo root) 
            => new DirectoryPackage(root);

        #endregion

        #region IEnumerable

        IEnumerator<IBlob> IEnumerable<IBlob>.GetEnumerator()
            => Enumerate().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => Enumerate().GetEnumerator();

        #endregion
    }
}