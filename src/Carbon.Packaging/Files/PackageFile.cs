using System;
using System.Collections.Generic;
using System.IO;

namespace Carbon.Packaging
{
    using Storage;

    public abstract class PackageFile : IBlob
    {
        public PackageFile(string name, long size, DateTime modified)
        {
            #region Preconditions

            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (name.Length == 0)
                throw new ArgumentException("May not be empty", nameof(name));

            if (size <= 0)
                throw new ArgumentException("Must be greater than 0", nameof(size));

            #endregion

            Name = name;
            Size = size;
            Modified = modified;
        }

        public string Name { get; }

        public long Size { get; }

        public DateTime Modified { get; }

        public IDictionary<string, string> Metadata => null;

        public abstract Stream Open();

        public virtual void Dispose() { }
    }
}