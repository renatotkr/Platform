using System.IO;

namespace Carbon.Packaging
{
    internal class VolumeFile : PackageFile
    {
        private readonly FileInfo file;

        public VolumeFile(string name, FileInfo file)
            : base(name, file.Length, file.LastWriteTimeUtc)
        {
            this.file = file;
        }

        public override Stream Open() 
            => file.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
    }
}