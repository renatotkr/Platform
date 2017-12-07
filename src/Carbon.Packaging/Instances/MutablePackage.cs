using System;
using System.Collections.Generic;
using System.IO;

namespace Carbon.Packaging
{
    using Storage;

    public class MutablePackage : Package
    {
        private readonly IPackage basePackage;
        private readonly List<IBlob> files = new List<IBlob>();

        public MutablePackage(IPackage basePackage)
        {
            this.basePackage = basePackage ?? throw new ArgumentNullException(nameof(basePackage));
        }

        public void Add(string key, Stream stream)
        {
            #region Preconditions

            if (key == null) throw new ArgumentNullException(nameof(key));

            #endregion

            var blob = new Blob(key, stream);

            files.Add(blob);
        }

        public override IEnumerable<IBlob> Enumerate()
        {
            foreach (var entry in basePackage)
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
            (basePackage as IDisposable)?.Dispose();
        }
    }
}