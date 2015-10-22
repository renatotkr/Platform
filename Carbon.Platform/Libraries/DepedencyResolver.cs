namespace Carbon.Libraries
{
    using System.Threading.Tasks;

    public class DepedencyResolver
    {
        private readonly ILibraryRegistry registry;

        public DepedencyResolver(ILibraryRegistry registry)
        {
            this.registry = registry;
        }

        public DependencyGraph Expand(Library package)
        {
            var graph = new DependencyGraph();

            Expand(package, graph);

            return graph;
        }

        private void Expand(Library package, DependencyGraph graph)
        {
            // Build up the graph
            foreach (var dep in package.Dependencies)
            {
                if (!dep.IsResolved) ResolveAsync(dep).Wait();

                var node = graph.FindOrAdd(dep);

                // TODO: Throw if there's a circular reference
                // TODO: Throw if a library asks for a version that conflicts with another library

                foreach (var childDep in dep.Dependencies)
                {
                    var childNode = graph.FindOrAdd(childDep);

                    node.AddEdge(childNode);

                    Expand(childDep, graph);
                }
            }
        }

        private async Task ResolveAsync(Library depedency)
        {
            var release = await registry.FindAsync(depedency.Name, depedency.Version).ConfigureAwait(false);

            depedency.Version = release.Version;
            depedency.Dependencies = release.Dependencies;
        }
    }
}