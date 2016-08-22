﻿namespace Carbon.Platform
{
    public class DepedencyResolver
    {
        private readonly ILibraryRegistry registry;

        public DepedencyResolver(ILibraryRegistry registry)
        {
            this.registry = registry;
        }

        public DependencyGraph<Library> Expand(Library package)
        {
            var graph = new DependencyGraph<Library>();

            Expand(package, graph);

            return graph;
        }

        private void Expand(Library package, DependencyGraph<Library> graph)
        {
            // Build up the graph
            foreach (var dep in package.Dependencies)
            {
                if (!dep.IsResolved) Resolve(dep);

                var node = graph.FindOrAdd(dep.Name, dep);

                // TODO: Throw if there's a circular reference
                // TODO: Throw if a library asks for a version that conflicts with another library

                foreach (var childDep in dep.Dependencies)
                {
                    var childNode = graph.FindOrAdd(childDep.Name, childDep);

                    node.AddEdge(childNode);

                    Expand(childDep, graph);
                }
            }
        }

        private void Resolve(Library depedency)
        {
            var release = registry.Find(depedency.Name, depedency.Version);

            depedency.Version = release.Version;
            depedency.Dependencies = release.Dependencies;
        }
    }
}