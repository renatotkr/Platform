using System;

namespace Carbon.Packaging
{
    public struct PutPackageResult
    {
        public PutPackageResult(
            string name, 
            byte[] iv, 
            byte[] sha256)
        {
            Name       = name   ?? throw new ArgumentNullException(nameof(name));
            IV         = iv     ?? throw new ArgumentNullException(nameof(iv));
            Sha256     = sha256 ?? throw new ArgumentNullException(nameof(sha256));
        }

        public PutPackageResult(string name, byte[] sha256)
        {
            Name       = name   ?? throw new ArgumentNullException(nameof(name));
            Sha256     = sha256 ?? throw new ArgumentNullException(nameof(sha256));
            IV         = null;
        }

        public string Name { get; }

        public byte[] IV { get; }

        public byte[] Sha256 { get; }
    }
}

// var key = tag.Path + ".zip";  (e.g. app/2.1.1.zip)