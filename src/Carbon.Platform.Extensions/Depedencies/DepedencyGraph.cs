using System;
using System.Collections.Generic;
using System.Linq;

namespace Carbon.Packaging
{
    public class DependencyGraph<T>
    {
        private readonly Dictionary<long, Node<T>> map = new Dictionary<long, Node<T>>();

        public IEnumerable<Node<T>> GetNodes() => map.Values;

        public Node<T> FindOrAdd(long id, T depedency)
        {
            if (!map.TryGetValue(id, out Node<T> node))
            {
                node = new Node<T>(depedency);

                map.Add(id, node);
            }

            return node;
        }

        private void VisitNode(Node<T> node)
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

        private bool IsFirstVisit(Node<T> node)
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

        private List<Node<T>> sortedNodes;
        private HashSet<Node<T>> visitedNodes;

        public IList<T> Sort()
        {
            sortedNodes = new List<Node<T>>();
            visitedNodes = new HashSet<Node<T>>();

            foreach (var node in GetNodes())
            {
                VisitNode(node);
            }

            return sortedNodes.Select(n => n.Value).ToArray();
        }
    }

    public class Node<T>
    {
        public Node(T value)
        {
            Value = value;
        }

        public T Value { get; }

        // Dependencies
        public List<Node<T>> Outgoing { get; } = new List<Node<T>>();

        public List<Node<T>> Incoming { get; } = new List<Node<T>>();

        // Lower level depedency
        // e.g. A (source) depends on C (target)
        public void AddEdge(Node<T> value)
        {
            Outgoing.Add(value);

            value.Incoming.Add(this);
        }
    }
}

// https://en.wikipedia.org/wiki/Dependency_graph
