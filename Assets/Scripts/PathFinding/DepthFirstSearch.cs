using System;
using System.Collections.Generic;
using System.Linq;

namespace Pathfinding
{
    public static class DepthFirstSearch
    {
        public static IEnumerable<TNode> Run<TNode>(TNode initialNode, Func<TNode, bool> satisfies, Func<TNode, IEnumerable<TNode>> expand)
        {
            Stack<TNode> openNodes = new Stack<TNode>();
            HashSet<TNode> closedNodes = new HashSet<TNode>();
            Dictionary<TNode, TNode> parents = new Dictionary<TNode, TNode>();

            openNodes.Push(initialNode);
            TNode currentNode;

            while (openNodes.Count > 0)
            {
                currentNode = openNodes.Pop();
                closedNodes.Add(currentNode);

                if (satisfies(currentNode))
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

                    openNodes.Push(node);
                    parents[node] = currentNode;
                }
            }

            return new List<TNode>() { initialNode };
        }
    }
}