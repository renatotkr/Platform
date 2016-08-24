using Carbon.Platform;

namespace Carbon.Packaging
{
    public class DepedencyResolver
    {
        private readonly IPackageRegistry registry;

        private readonly DependencyGraph<IPackage> graph = new DependencyGraph<IPackage>();

        public DepedencyResolver(IPackageRegistry registry)
        {
            this.registry = registry;
        }

        private void Expand(IPackage package)
        {
            // Build up the graph
            foreach (var dep in package.Dependencies)
            {
                if (dep.ResolvedPackage == null)
                {
                    dep.ResolvedPackage = Resolve(dep);
                }

                var node = graph.FindOrAdd(dep.PackageId, dep.ResolvedPackage);

                // TODO: Throw if there's a circular reference
                // TODO: Throw if a library asks for a version that conflicts with another library
                
                foreach (var childDep in dep.ResolvedPackage.Dependencies)
                {    
                    var childNode = graph.FindOrAdd(childDep.PackageId, childDep.ResolvedPackage);

                    node.AddEdge(childNode);

                    Expand(childDep.ResolvedPackage);
                }
            }
        }

        private IPackage Resolve(PackageDependency depedency)
        {
            return registry.FindAsync(depedency.DepedencyName, depedency.DependencyVersion).Result;
        }
    }
}