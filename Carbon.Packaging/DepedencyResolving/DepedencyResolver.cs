using Carbon.Platform;

namespace Carbon.Packaging
{
    public class DepedencyResolver
    {
        private readonly IPackageRegistry registry;

        public DepedencyResolver(IPackageRegistry registry)
        {
            this.registry = registry;
        }

        public DependencyGraph<IPackage> Expand(LibraryRelease package)
        {
            var graph = new DependencyGraph<IPackage>();

            Expand(package, graph);

            return graph;
        }

        private void Expand(LibraryRelease package, DependencyGraph<IPackage> graph)
        {
            // Build up the graph
            foreach (var dep in package.Dependencies)
            {
                if (dep.ResolvedLibrary == null)
                {
                    dep.ResolvedLibrary = Resolve(dep);
                }

                var node = graph.FindOrAdd(dep.LibraryId, dep.ResolvedLibrary);

                // TODO: Throw if there's a circular reference
                // TODO: Throw if a library asks for a version that conflicts with another library
                
                foreach (var childDep in dep.ResolvedLibrary.Dependencies)
                {    
                    var childNode = graph.FindOrAdd(childDep.LibraryId, childDep.ResolvedLibrary);

                    node.AddEdge(childNode);

                    Expand(childDep.ResolvedLibrary, graph);
                }
            }
        }

        private LibraryRelease Resolve(LibraryDepedency depedency)
        {
            return null;
            // TODO
            // return registry.FindAsync(depedency.LibraryId, depedency.Version).Result;
        }
    }
}