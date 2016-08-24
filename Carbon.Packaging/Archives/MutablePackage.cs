using System;
using System.Collections.Generic;
using System.IO;

namespace Carbon.Packaging
{
    public class MutablePackage : Package
    {
        private readonly Package basePackage;
        private readonly List<IFile> files = new List<IFile>();

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

        public override IEnumerable<IFile> Enumerate()
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