using System;
using System.Collections.Generic;
using System.Linq;

namespace Pathfinding
{
    public static class Dijkstra
    {
        public static IEnumerable<TNode> Run<TNode>(TNode initialNode, Func<TNode, bool> satisfites, Func<TNode, IEnumerable<Tuple<TNode, float>>> expand)
        {
            HashSet<TNode> openNodes = new HashSet<TNode>();
            HashSet<TNode> closedNodes = new HashSet<TNode>();
            Dictionary<TNode, TNode> parents = new Dictionary<TNode, TNode>();
            Dictionary<TNode, float> costs = new Dictionary<TNode, float>();

            openNodes.Add(initialNode);
            costs[initialNode] = 0;

            TNode currentNode;
            float currentCost;

            while (openNodes.Count > 0)
            {
                currentNode = openNodes.OrderBy((n) => costs[n]).First();

                if (satisfites(currentNode))
                {
                    return Utility.Generator(currentNode, (n) => parents[n])
                                  .TakeWhile((n) => parents.ContainsKey(n))
                                  .Reverse();
                }

                currentCost = costs[currentNode];

                openNodes.Remove(currentNode);
                closedNodes.Add(currentNode);

                foreach (Tuple<TNode, float> node in expand(currentNode))
                {
                    if (closedNodes.Contains(node.Item1) || (costs.ContainsKey(node.Item1) && currentCost + node.Item2 > costs[node.Item1]))
                    {
                        continue;
                    }

                    openNodes.Add(node.Item1);
                    parents[node.Item1] = currentNode;
                    costs[node.Item1] = currentCost + node.Item2;
                }
            }

            return new List<TNode>() { initialNode };
        }
    }
}