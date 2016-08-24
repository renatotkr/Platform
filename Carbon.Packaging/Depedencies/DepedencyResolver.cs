using System.Threading.Tasks;

namespace Carbon.Packaging
{
    public class DepedencyResolver
    {
        private readonly IPackageRegistry registry;

        private readonly DependencyGraph<IPackage> graph;

        public DepedencyResolver(IPackageRegistry registry)
        {
            this.registry = registry;
            this.graph = new DependencyGraph<IPackage>();
        }

        private async Task ExpandAsync(IPackage package)
        {
            // Build up the graph
            foreach (var dep in package.Dependencies)
            {
                if (dep.ResolvedPackage == null)
                {
                    dep.ResolvedPackage = await ResolveAsync(dep).ConfigureAwait(false);
                }

                var node = graph.FindOrAdd(dep.PackageId, dep.ResolvedPackage);

                // TODO: Throw if there's a circular reference
                // TODO: Throw if a library asks for a version that conflicts with another library
                
                foreach (var childDep in dep.ResolvedPackage.Dependencies)
                {    
                    var childNode = graph.FindOrAdd(childDep.PackageId, childDep.ResolvedPackage);

                    node.AddEdge(childNode);

                    await ExpandAsync(childDep.ResolvedPackage).ConfigureAwait(false);
                }
            }
        }

        private async Task<IPackage> ResolveAsync(PackageDependency dep)
        {
            return await registry.FindAsync(dep.DependencyId, dep.DependencyVersion).ConfigureAwait(false);
        }
    }
}