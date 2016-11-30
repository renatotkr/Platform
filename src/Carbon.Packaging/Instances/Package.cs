using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Carbon.Packaging
{
    using Storage;

    public abstract class Package : IEnumerable<IBlob>, IDisposable
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

        public IBlob[] List(string prefix)
            => Enumerate().Where(item => item.Name.StartsWith(prefix)).ToArray();

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