using System;
using System.Collections.Generic;
using System.IO;

namespace Carbon.Packaging
{
    using Storage;

    public class MutablePackage : Package
    {
        private readonly Package basePackage;
        private readonly List<IBlob> files = new List<IBlob>();

        public MutablePackage(Package basePackage)
        {
            #region Preconditions

            if (basePackage == null) throw new ArgumentNullException(nameof(basePackage));

            #endregion

            this.basePackage = basePackage;
        }

        public void Add(string name, Stream stream)
        {
            files.Add(new MemoryFile(name, stream));
        }

        public override IEnumerable<IBlob> Enumerate()
        {
            foreach (var entry in basePackage.Enumerate())
            {
                yield return entry;
            }

            foreach (var entry in files)
            {
                yield return entry;
            }
        }

        public override void Dispose()
        {
            basePackage.Dispose();
        }
    }
}