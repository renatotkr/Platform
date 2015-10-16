namespace Carbon.Libraries
{
    using System.Collections.Generic;

    public class DepedencyResolver
    {
        // Given a set of depedencies, solve for a common set.
        // TODO: Ensure that these are ordered correctly (i.e. a depedency is always included BEFORE the library)
        // If the solution is unsolvable, throw.

        public LibraryRelease[] Solve(LibraryRelease[] releases)
        {
            var solution = new Solution();

            foreach (var release in releases)
            {
                var dep = solution.FindOrAdd(release.Name, release);

                foreach (var d in dep.Dependencies)
                {
                    // TODO: Lookup
                }

            }

            return null;    
        }

    }

    public class Solution
    {
        private readonly Dictionary<string, LibraryRelease> libs = new Dictionary<string, LibraryRelease>();

        public void Add(LibraryRelease release)
        {
            libs.Add(release.Name, release);
        }

        public LibraryRelease FindOrAdd(string name, LibraryRelease releaseToAdd)
        {
            LibraryRelease release;

            if (!libs.TryGetValue(name, out release))
            {
                libs.Add(name, releaseToAdd);

                return releaseToAdd;
            }

            return release;
        }
    }

    public class Dependency
    {
        public string Name { get; set; }

        public string Version { get; set; }

        public int Order { get; set; }

        
        public LibraryRelease Release { get; set; }
    }


}
