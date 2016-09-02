using System.Configuration;
using System.IO;

namespace Carbon.Hosting
{
    public class ProgramEnvironment
    {
        private readonly DirectoryInfo root;

        public ProgramEnvironment()
        {
            this.root = new DirectoryInfo(ConfigurationManager.AppSettings["appsRoot"] ?? "Z:/apps/");
        }

        public ProgramEnvironment(DirectoryInfo root)
        {
            this.root = root;
        }

        public string[] Roles { get; set; }

        // Environment

        public DirectoryInfo Root => root;
    }
}