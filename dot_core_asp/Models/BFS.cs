using System;
using System.Collections.Generic;
using System.Linq;

namespace dot_core_asp.Models
{
    public class BFS
    {
        public void BFSIterative(Graph G, Node s)
        {
            Stack StackElements = new Stack();
            StackElements.Push(s.ResidingNode);

            s.ResidingNode.Visited = true;
            G.PrintVisitedVertices();

            while (StackElements.Count != 0)
            {
                // Assume that a node in graph can have many children.
                var v = StackElements.Bottom();
                var nodeOfv = G.GetNode(v);
                StackElements.PopBottom();

                foreach (var child in nodeOfv.Children)
                {
                    if ((child != null) && !child.Visited)
                    {
                        StackElements.Push(child);
                        child.Visited = true;
                    }
                    G.PrintVisitedVertices();
                }
                if (G.CountVisitedNode == G.Nodes.Count())
                    break;
                // G.PrintVisitedVertices();
            }
        }

        public void TestBFS()
        {
            var graph = new Graph();
            var rootNode = graph.Nodes[0];
            BFSIterative(graph, rootNode);
        }
        
    }
}