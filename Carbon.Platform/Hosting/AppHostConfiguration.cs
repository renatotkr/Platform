using System.Configuration;
using System.IO;

namespace Carbon.Platform.Hosting
{
    public class AppHostConfiguration
    {
        private readonly DirectoryInfo root;

        public AppHostConfiguration()
        {
            this.root = new DirectoryInfo(ConfigurationManager.AppSettings["appsRoot"] ?? "Z:/apps/");
        }

        public AppHostConfiguration(DirectoryInfo root)
        {
            this.root = root;
        }

        public string[] Roles { get; set; }

        // Environment

        public DirectoryInfo Root => root;
    }
}