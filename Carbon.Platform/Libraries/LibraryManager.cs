namespace Carbon.Libraries
{
    public class LibraryManager
    {
        public LibraryRelease Query(string name, Semver version)
        {
            switch (version.Level)
            {
                case MatchLevel.Patch: // find exact
                case MatchLevel.Minor: // find highest matching major & minor
                case MatchLevel.Major: // find higest matching major 
                    break;
            }

            return null;
        }

        public LibraryRelease PublishAsync(Library library, Semver version)
        {
            // - Get source from GIT
            // - Verify source
            // - Read package.json
            // As meeded
            // - Extract TypeScript and build
            // - Extract SASS and build

            // Create release record

            // Make sure version is incrimental within the series

            // Push to CDN

            // OK

            return null;
        }

    }
}
