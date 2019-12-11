using System;
using System.Collections.Generic;
using System.Linq;

namespace dot_core_asp.Models
{
    public class BFS
    {
        public void BFSIterative(Graph graph, Node node)
        {
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