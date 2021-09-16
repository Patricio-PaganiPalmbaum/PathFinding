using System;
using System.Collections.Generic;
using System.Linq;

namespace Pathfinding
{
    public static class BreadthFirstSearch
    {
        public static IEnumerable<TNode> Run<TNode>(TNode initialNode, Func<TNode, bool> satifies, Func<TNode, IEnumerable<TNode>> expand)
        {
            Queue<TNode> openNodes = new Queue<TNode>();
            HashSet<TNode> closedNodes = new HashSet<TNode>();
            Dictionary<TNode, TNode> parents = new Dictionary<TNode, TNode>();

            openNodes.Enqueue(initialNode);

            TNode currentNode;

            while (openNodes.Count > 0)
            {
                currentNode = openNodes.Dequeue();
                closedNodes.Add(currentNode);

                if (satifies(currentNode))
                {
                    return Utility.Generator(currentNode, (n) => parents[n])
                                  .TakeWhile((n) => parents.ContainsKey(n))
                                  .Reverse();
                }

                foreach (TNode node in expand(currentNode))
                {
                    if (openNodes.Contains(node) || closedNodes.Contains(node))
                    {
                        continue;
                    }

                    openNodes.Enqueue(node);
                    parents[node] = currentNode;
                }

            }

            return new List<TNode>() { initialNode };
        }
    }
}