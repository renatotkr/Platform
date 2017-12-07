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

        #region Helpers

        public static Package FromDirectory(DirectoryInfo root) => new DirectoryPackage(root);

        #endregion

        #region IEnumerable

        public IEnumerator<IBlob> GetEnumerator() => Enumerate().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Enumerate().GetEnumerator();

        #endregion
    }
}