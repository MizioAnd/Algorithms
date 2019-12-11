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
                // Assume that a node can have many children. Define new type child and create List<Vertex> for all children with increasing indexing.
                var v = StackElements.Top();
                var nodeOfv = G.GetNode(v);

                foreach (var child in nodeOfv.Children)
                {
                    if (!child.Visited)
                    {
                        StackElements.Push(child);
                        child.Visited = true;
                    }
                    else
                    {
                        StackElements.Push(G.GetNode(child).Parent);
                        G.GetNode(child).Parent.Visited = true;
                    }
                }

            }


            throw new NotImplementedException();
        }

        public void TestBFS()
        {
            var graph = new Graph();
            var rootNode = graph.Nodes[0];
            BFSIterative(graph, rootNode);

        }
        
    }
}