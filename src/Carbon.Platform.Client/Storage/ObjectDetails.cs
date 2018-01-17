using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Carbon.Storage;

namespace Carbon.Platform.Storage
{
    public class ObjectDetails : IBlob
    {
        public string Key { get; set; }

        public long Size { get; set; }

        public DateTime Modified { get; set; }

        public long Version { get; set; }

        public IReadOnlyDictionary<string, string> Properties { get; set; }

        public void Dispose() { }

        public ValueTask<Stream> OpenAsync()
        {
            throw new NotImplementedException();
        }
    }
}