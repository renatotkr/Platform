namespace Carbon.Libraries
{
    using System.Collections.Generic;
    using System.Linq;

    // https://en.wikipedia.org/wiki/Dependency_graph

    public class DependencyGraph
    {
        private readonly Dictionary<string, Node<Library>> map = new Dictionary<string, Node<Library>>();

        public IEnumerable<Node<Library>> GetNodes()
        {
            return map.Values;
        }

        public Node<Library> FindOrAdd(Library depedency)
        {
            Node<Library> node;

            if (!map.TryGetValue(depedency.Name, out node))
            {
                node = new Node<Library> {
                    Value = depedency
                };

                map.Add(depedency.Name, node);
            }

            return node;
        }

        private readonly List<Node<Library>> sortedNodes = new List<Node<Library>>();

        private HashSet<Node<Library>> visitedNodes = new HashSet<Node<Library>>();

        private void VisitNode(Node<Library> node)
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

        internal bool IsFirstVisit(Node<Library> node)
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

        public IList<Library> Sort()
        {
            foreach (var node in GetNodes())
            {
                VisitNode(node);
            }

            return sortedNodes.Select(n => n.Value).ToArray();
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
}