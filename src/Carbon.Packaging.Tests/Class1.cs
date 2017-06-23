using System.IO;
using System.Threading.Tasks;

using Xunit;

namespace Carbon.Packaging.Tests
{
    public class TarTests
    {
        [Fact]
        public async Task A()
        {
            var package = Package.FromDirectory(new DirectoryInfo(@"E:\d\"));

            using (var fs = File.OpenWrite(@"E:\package.tar.gz"))
            {
                await package.ToTarStreamAsync(fs);
            }

            using (var fs = File.OpenWrite(@"E:\package.zip"))
            {
                await package.ToZipStreamAsync(fs);
            }

            using (var fs3 = File.OpenRead(@"E:\package.tar.gz"))
            {
                var tar = await TarPackage.OpenAsync(fs3, stripFirstLevel: false, leaveStreamOpen: true);

                // throw new Exception(string.Join(",", tar.Select(f => f.Name)));

                File.Delete(@"E:\package2.zip");

                using (var fs4 = File.OpenWrite(@"E:\package2.zip"))
                {
                    await tar.ToZipStreamAsync(fs4, leaveStreamOpen: true);
                }
            }

            using (var fs3 = File.OpenRead(@"E:\package.tar.gz"))
            {
                var tar = await TarPackage.OpenAsync(fs3, stripFirstLevel: false, leaveStreamOpen: true);

                // throw new Exception(string.Join(",", tar.Select(f => f.Name)));

                File.Delete(@"E:\package2.tar.gz");

                using (var fs5 = File.OpenWrite(@"E:\package2.tar.gz"))
                {
                    await tar.ToTarStreamAsync(fs5, leaveStreamOpen: true);
                }
            }
        }
    }
}
