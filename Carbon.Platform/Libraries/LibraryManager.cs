namespace Carbon.Libraries
{
    using System.Threading.Tasks;

    using Carbon.Platform;

    public class LibraryManager
    {
        public Library Find(string name, Semver version)
        {
            // What store should we query?

            switch (version.Level)
            {
                case VersionCategory.Patch: // find exact
                case VersionCategory.Minor: // find highest matching major & minor
                case VersionCategory.Major: // find higest matching major 
                    break;
            }

            // Get release depedencies

            return null;
        }
        
        public async Task<Library> PublishAsync(Library library, Semver version)
        {
            // - Get source from GIT
            // - Verify source
            // - Read package.json

            // Make sure version is incrimental within the series

            // As needed
            // - Extract TypeScript and build
            // - Extract SASS and build

            // Push to CDN

            // Create release record
            // Where? DynamoDb or RelationalDB?

            // TODO: return record
            return null;
        }

    }
}
