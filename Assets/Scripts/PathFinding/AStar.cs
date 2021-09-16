using System;
using System.Collections.Generic;
using System.Linq;

namespace Pathfinding
{
    public static class AStar
    {
        public static IEnumerable<TNode> Run<TNode>(TNode initialNode,
                                              Func<TNode, bool> satisfies,
                                              Func<TNode, IEnumerable<Tuple<TNode, float>>> expand,
                                              Func<TNode, float> heuristic)
        {
            HashSet<TNode> openNodes = new HashSet<TNode>();
            HashSet<TNode> closedNodes = new HashSet<TNode>();
            Dictionary<TNode, TNode> parents = new Dictionary<TNode, TNode>();
            Dictionary<TNode, float> gCost = new Dictionary<TNode, float>();
            Dictionary<TNode, float> hCost = new Dictionary<TNode, float>();

            openNodes.Add(initialNode);
            gCost[initialNode] = 0;
            hCost[initialNode] = heuristic(initialNode);

            TNode currentNode;
            float currentCost;

            while (openNodes.Count > 0)
            {
                currentNode = openNodes.OrderBy((n) => hCost[n]).First();

                if (satisfies(currentNode))
                {
                    return Utility.Generator(currentNode, (n) => parents[n])
                                  .TakeWhile((n) => parents.ContainsKey(n))
                                  .Reverse();
                }

                currentCost = gCost[currentNode];

                openNodes.Remove(currentNode);
                closedNodes.Add(currentNode);

                foreach (Tuple<TNode, float> node in expand(currentNode))
                {
                    if (closedNodes.Contains(node.Item1) || (gCost.ContainsKey(node.Item1) && currentCost + node.Item2 > gCost[node.Item1]))
                    {
                        continue;
                    }

                    openNodes.Add(node.Item1);
                    parents[node.Item1] = currentNode;
                    gCost[node.Item1] = currentCost + node.Item2;
                    hCost[node.Item1] = currentCost + node.Item2 + heuristic(node.Item1);
                }
            }


            return new List<TNode>() { initialNode};
        }
    }
}