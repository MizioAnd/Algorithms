using System;
using System.Collections.Generic;
using System.Linq;

namespace dot_core_asp.Models
{
    public class BFS
    {
        public IList<IList<Vertex>> Paths {get; set;}

        public void BFSIterative(Graph G, Node s)
        {
            Stack StackElements = new Stack();
            StackElements.Push(s.ResidingNode);

            s.ResidingNode.Visited = true;
            G.PrintVisitedVertices();

            Paths = new List<IList<Vertex>>(){new List<Vertex>(){s.ResidingNode} };
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

                        // Foreach child create a new list of an existing list with end element equal to vertex v and add child to it and afterwards add lists to List<List<vertex>>
                        PatchChildToList(child, v, Paths);
                    }
                    // G.PrintVisitedVertices();
                }
                if (G.CountVisitedNode == G.Nodes.Count())
                    break;
                // G.PrintVisitedVertices();      
            }
        }

        private void PatchChildToList(Vertex child, Vertex parent, IList<IList<Vertex>> paths)
        {
            var path = paths.Where(x => x.Contains(parent) && x.Last() == parent).FirstOrDefault();
            var pathNew = new List<Vertex>(path);
            pathNew.Add(child);
            paths.Add(pathNew);
            Console.WriteLine(String.Join(";", pathNew.Select(x => x.idxGrid)));
        }

        public void TestBFS()
        {
            var graph = new Graph();
            var rootNode = graph.Nodes[0];
            BFSIterative(graph, rootNode);
        }
        
    }
}