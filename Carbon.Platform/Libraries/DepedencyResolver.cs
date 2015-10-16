namespace Carbon.Libraries
{
    using System.Collections.Generic;

    public interface IPackage
    {
        Dependency[] Dependencies { get; }
    }

    public class DepedencyResolver
    {
        // Given a set of depedencies, solve for a common set.

        private readonly LibraryManager manager = new LibraryManager();

        public DependencyGraph Expand(IPackage package)
        {
            var graph = new DependencyGraph();

            Expand(package, graph);

            return graph;
        }

        private void Expand(IPackage package, DependencyGraph graph)
        {
            // Build up the graph

            foreach (var dep in package.Dependencies)
            {
                var node = graph.FindOrAdd(dep);

                // TODO: Throw if there's a circular reference
                // TODO: Throw if a library asks for a version that conflicts with another library

                foreach (var childDep in dep.Dependencies)
                {
                    var childNode = graph.FindOrAdd(childDep);

                    node.AddEdge(childNode);

                    if (childDep.Dependencies.Length > 0)
                    {
                        Expand(childDep, graph);
                    }
                }
            }
        }

        public LibraryRelease Resolve(Dependency depedency)
        {
            depedency.Release = manager.Find(depedency.Name, depedency.Version);

            return depedency.Release;
        }
    }

    // https://en.wikipedia.org/wiki/Dependency_graph

    public class DependencyGraph
    {
        private readonly Dictionary<string, Node<Dependency>> map = new Dictionary<string, Node<Dependency>>();

        public Node<Dependency> FindOrAdd(Dependency depedency)
        {
            Node<Dependency> node;

            if (!map.TryGetValue(depedency.Name, out node))
            {
                node = new Node<Dependency> {
                    Value = depedency
                };

                map.Add(depedency.Name, node);
            }

            return node;
        }

        // It is possible to derive an evaluation order or the absence of an evaluation order that respects the given dependencies from the dependency graph.

        // http://www.electricmonk.nl/docs/dependency_resolving_algorithm/dependency_resolving_algorithm.html

        // Topological_sorting
        // https://en.wikipedia.org/wiki/Topological_sorting

        public LibraryRelease[] Sort()
        {
            // use a topological sorting algorithm

            return null;
        }
    }

    public class Node<T>
    {
        public T Value { get; set; }

        // Dependencies
        public List<Edge<T>> Outgoing { get; } = new List<Edge<T>>();
        
        public List<Edge<T>> Incoming { get; } = new List<Edge<T>>();

        // Lower level depedency
        // e.g. A (source) depends on C (target)
        public void AddEdge(Node<T> value)
        {
            // Add Outgoing Edge
            Outgoing.Add(new Edge<T>(this, value));

            // Add Incoming Edge (Reverse)
            value.Incoming.Add(new Edge<T>(value, this));
        }
    }

    public class Edge<T>
    {
        // source -> target

        public Edge(Node<T> source, Node<T> target)
        {
            Source = source;
            Target = target;
        }

        public Node<T> Source { get; } 

        public Node<T> Target { get; }
    }
   

    public class Dependency : IPackage
    {
        public string Name { get; set; }

        public Semver Version { get; set; }

        public Dependency[] Dependencies { get; set; }

        public LibraryRelease Release { get; set; }
    }


}
