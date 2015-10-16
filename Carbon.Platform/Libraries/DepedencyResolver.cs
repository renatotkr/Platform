namespace Carbon.Libraries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface ILibrary
    {
        Dependency[] Dependencies { get; }
    }

    public class DepedencyResolver
    {
        private readonly LibraryManager manager = new LibraryManager();

        public DependencyGraph Expand(ILibrary package)
        {
            var graph = new DependencyGraph();

            Expand(package, graph);

            return graph;
        }

        private void Expand(ILibrary package, DependencyGraph graph)
        {
            // Build up the graph
            foreach (var dep in package.Dependencies)
            {
                if (!dep.IsResolved) Resolve(dep);

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

        public IEnumerable<Node<Dependency>> GetNodes()
        {
            return map.Values;
        }

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

        private readonly List<Node<Dependency>> sortedNodes = new List<Node<Dependency>>();

        private HashSet<Node<Dependency>> visitedNodes = new HashSet<Node<Dependency>>();

        private void VisitNode(Node<Dependency> node)
        {
            if (IsFirstVisit(node))
            {
                foreach (var dependencyNode in node.Outgoing.Select(dep => dep))
                {
                    VisitNode(dependencyNode);
                }

                sortedNodes.Add(node);
            }
        }

        internal bool IsFirstVisit(Node<Dependency> node)
        {
            var isFirstVisit = !visitedNodes.Contains(node);

            if (isFirstVisit)
            {
                visitedNodes.Add(node);
            }

            return isFirstVisit;
        }

        // It is possible to derive an evaluation order or the absence of an evaluation 
        // order that respects the given dependencies from the dependency graph.

        // ... using
        // Topological sorting (https://en.wikipedia.org/wiki/Topological_sorting)

        public IList<LibraryRelease> Sort()
        {
            foreach (var node in GetNodes())
            {
                VisitNode(node);
            }

            return sortedNodes.Select(n => n.Value.Release).ToArray();
        }
    }

    public class Node<T>
    {
        public T Value { get; set; }

        // Dependencies
        public List<Node<T>> Outgoing { get; } = new List<Node<T>>();
        
        public List<Node<T>> Incoming { get; } = new List<Node<T>>();
        
        // Lower level depedency
        // e.g. A (source) depends on C (target)
        public void AddEdge(Node<T> value)
        {
            // Add Outgoing Edge
            Outgoing.Add(value);

            // Add Incoming Edge (Reverse)
            value.Incoming.Add(this);
        }
    }

    public class Dependency : ILibrary
    {
        public string Name { get; set; }

        public Semver Version { get; set; }

        public Dependency[] Dependencies { get; set; }

        public bool IsResolved { get; set; }

        // Null until resolved
        public LibraryRelease Release { get; set; }
    }
}