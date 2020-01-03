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

            // var layerCountSlow = 1;
            // var oldStackCount = 1;
            // var diff = 0;
            // var childrenSameDiff = 0;
            Paths = new List<IList<Vertex>>(){new List<Vertex>(){s.ResidingNode} };
            while (StackElements.Count != 0)
            {
                // Todo: group nodes layers to determine length of shortest path.

                // Console.WriteLine("Stack:{0}", oldStackCount);
                // Console.WriteLine("layer:{0}", layerCountSlow);

                // if (childrenSameDiff > 0)
                //     diff = childrenSameDiff;
                //     childrenSameDiff -= 1;
                // if (diff == 0)
                // {
                //     layerCountSlow += 1;
                // }
                // if (diff == 0 & childrenSameDiff > 0)
                //     childrenSameDiff -= 1;

                // if (diff >= 1)
                //     diff -= 1;

                // if (StackElements.Count != oldStackCount)
                // {}
                // if (StackElements.Count - oldStackCount >= 1)
                // {
                //     diff = StackElements.Count - oldStackCount;
                // }

                // Assume that a node in graph can have many children.
                var v = StackElements.Bottom();
                var nodeOfv = G.GetNode(v);
                // oldStackCount = StackElements.Count;


                StackElements.PopBottom();

                foreach (var child in nodeOfv.Children)
                {
                    if ((child != null) && !child.Visited)
                    {
                        StackElements.Push(child);
                        child.Visited = true;

                        // if (diff > 0)
                        //     childrenSameDiff += 1;

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