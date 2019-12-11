using System;
using System.Collections.Generic;
using System.Linq;

namespace dot_core_asp.Models
{
    public class Stack
    {
        private List<Vertex> StackElements;
        public int Count 
        {
            get { return StackElements.Count; }
            // private set { this.Count = value; }
        }

        public Stack()
        {
            StackElements = new List<Vertex>();
            // Count = 0;
        }

        public void Push(Vertex s)
        {
            this.StackElements.Add(s);
        }
    
        public Vertex Top()
        {
            return this.StackElements.LastOrDefault();
        }
    
        public void Pop()
        {
            this.StackElements.RemoveAt(this.StackElements.Count - 1);
        }
    }

    public class Node 
    {
        public Vertex ResidingNode {get; set;}
        public Vertex Child1 {get; set;}
        public Vertex Child2 {get; set;}
        public Vertex Parent {get; set;}

        public Node()
        {

        }
    }

    public class Graph
    {
        public List<List<Vertex>> NodesNew {get; private set;}
        public List<Node> Nodes {get; private set;}
        public List<Vertex> verticesInTree {get; private set;}
        public int CountVisitedNode => Nodes.Where(x => x.ResidingNode.Visited).Count();

        public Graph()
        {
            Nodes = new List<Node>();
            NodesNew = new List<List<Vertex>>();
            verticesInTree = new List<Vertex>();
            FillGraph();
        }

        public void FillGraph()
        {
            // Build a Tree graph 
            // Adjacency list. A list of lists where each of element of the first list is A_i a list which contains all 
            // the vertices adjacent to vertex i.
            // Correction: Adjacency list elements consists of list starting with vertex in mind i and then any of adjacent 
            // vertices of vertex i sharing an edge. This structure is similar to the Node object used.

            // Hence the equation for the j-th level is
            // 2^j, .., 2^(j+1)-1
            // at 0-th level, j=0.
            // 1, .., 2^(0+1)-1 = 1, .., 1 = 1
            // Create 3 level Graph
            var endLevel = 2;
            var rangeLevels = Enumerable.Range(0, endLevel + 1);
            
            // Create all nodes and collect them in a list
            var countNodes = rangeLevels.Select(x => (int)Math.Pow(2, x)).Sum();
            verticesInTree = new List<Vertex>(countNodes);
            foreach (var idx_vertex in Enumerable.Range(1, countNodes))
            {
                var vertex = new Vertex();
                vertex.idx = idx_vertex;
                verticesInTree.Add(vertex);
            }

            foreach (var level in rangeLevels)
            {   
                IEnumerable<int> rangeNodeIndices;
                if (level == 0)
                {
                    var level_plus_one = level + 1;
                    rangeNodeIndices = Enumerable.Range((int)Math.Pow(2, level_plus_one), (int)Math.Pow(2, level_plus_one)).Where(i => i % 2 == 0);
                    foreach (var idx in rangeNodeIndices)
                    {
                        var node = new Node();
                        node.ResidingNode = verticesInTree[level];
                        node.Child1 = verticesInTree[idx - 1];
                        node.Child2 = verticesInTree[idx];

                        Nodes.Add(node);
                    }
                }
                else
                {
                    var level_minus_one = level - 1;
                    var rangeNodeIndices_parent = Enumerable.Range((int)Math.Pow(2, level_minus_one), (int)Math.Pow(2, level_minus_one));

                    rangeNodeIndices = Enumerable.Range((int)Math.Pow(2, level), (int)Math.Pow(2, level));

                    var level_plus_one = level + 1;
                    var rangeNodeIndicesChild = Enumerable.Range((int)Math.Pow(2, level_plus_one), (int)Math.Pow(2, level_plus_one)).Where(i => i % 2 == 0);

                    var iteSlow = 0;
                    var ite = 0;
                    foreach (var jte in Enumerable.Range(0, rangeNodeIndices.Count()))
                    {
                        var idx_current = rangeNodeIndices.ElementAt(jte);
                        var idx_child = rangeNodeIndicesChild.ElementAt(ite);
                        var idx_parent = rangeNodeIndices_parent.ElementAt(iteSlow);
                        var node = new Node();

                        node.ResidingNode = verticesInTree[idx_current - 1];

                        if (level != endLevel)
                        {
                            node.Child1 = verticesInTree[idx_child - 1];
                            node.Child2 = verticesInTree[idx_child];
                        }
                        node.Parent = verticesInTree[idx_parent - 1];

                        Nodes.Add(node);
                        ite += 1;
                        if ((jte + 1) % 2 == 0) { iteSlow += 1; }
                    }
                }              
            }
        }
    }

    public class Vertex
    {   
        public bool Visited {get; set;} = false;
        public int idx {get; set;}
    }

    /// <summary>
    /// The DFS algorithm is a recursive algorithm that uses the idea of backtracking. It involves exhaustive searches of all the nodes by going ahead, if possible, else by backtracking.
    /// Let G be a graph and let s be a source vertex
    /// Called once and method runs through tree iteratively.
    /// </summary>
    /// <param name="s">s is the starting vertex in the search</param>
    /// <param name="G">G is the graph that is searched</param>
    public class DFS
    {
        public void DFSIterative(Graph G, Node s)
        {
            // Todo: does not correctly visit root node when starting out at different node.

            // List<Vertex> stack = new List<Vertex>();
            // stack.Add(s);

            Stack StackElements = new Stack();
            StackElements.Push(s.ResidingNode);

            s.ResidingNode.Visited = true;

            PrintVisitedVertices(G);
            while (StackElements.Count != 0)
            {
                var v = StackElements.Top();
                var nodeOfv = GetNode(G, v);
                StackElements.Pop();

                // For all neighbours w of v in Graph G check if w is marked visited and push on to stack if not visited.
                // var neighboursOfv = GetNeighbours(G, v);

                // foreach (var neighbourNode in neighboursOfv)
                // {
                //     if (!neighbourNode.ResidingNode.Visited)
                //     {   
                //         StackElements.Push(neighbourNode.ResidingNode);
                //         neighbourNode.ResidingNode.Visited = true;
                //     }
                // }

                if ((nodeOfv.Child1 != null) && !nodeOfv.Child1.Visited)
                {
                    StackElements.Push(nodeOfv.Child1);
                    nodeOfv.Child1.Visited = true;
                }
                else if ((nodeOfv.Child2 != null) && !nodeOfv.Child2.Visited)
                {
                    StackElements.Push(nodeOfv.Child2);
                    nodeOfv.Child2.Visited = true;
                }
                else
                {
                    StackElements.Push(nodeOfv.Parent);
                    nodeOfv.Parent.Visited = true;
                }

                PrintVisitedVertices(G);

                // Break when all nodes are marked as visited.
                if (G.CountVisitedNode == G.Nodes.Count())
                    break;


                // foreach (var node in G.Nodes)
                // {
                //     if (!node.Child1.Visited)
                //     {
                //         StackElements.Push(node.Child1);
                //         node.Child1.Visited = true;
                //     }

                //     // Todo: Child2 should be visited in case that node more leafs occur in path and after a back move.

                //     // else if (!node.Child2.Visited)
                //     // {
                //     //     StackElements.Push(node.Child2);
                //     //     node.Child2.Visited = true;
                //     // }
                // }


            }
        }

        public void PrintVisitedVertices(Graph graph)
        {
            Console.WriteLine(String.Join("; ", graph.Nodes.Where(x => x.ResidingNode.Visited).Select(x => x.ResidingNode.idx).ToList()));
        }

        public List<Node> GetNeighbours(Graph G, Vertex vertex)
        {
            var neighbours = new List<Node>();
            foreach (var node in G.Nodes)
            {       
                if (node.ResidingNode.idx == vertex.idx)
                {
                    neighbours.Add(G.Nodes.Where(x => x.ResidingNode.idx == node.Child1.idx).FirstOrDefault());
                }
                    
            }
            return neighbours;
        }

        public Node GetNode(Graph graph, Vertex vertex)
        {
            var node = graph.Nodes.Where(x => x.ResidingNode.idx == vertex.idx).FirstOrDefault();
            return node;
        }

        public void DFSRecursive(Graph graph, Vertex vertex)
        {
            // Todo: currently not tested succesfully.
            vertex.Visited = true;

            // for all neighbours vertex of s in Graph G:
            // one critical problem is that if any node has shared indices like any parents chilc or vice versa then you also have to 
            // mark their vertex as visited.
            // Todo: how to mark overlapping nodes as visited?
            foreach (var node in graph.Nodes)
            {
                if (!node.Child1.Visited)
                {
                    DFSRecursive(graph, node.Child1);
                }
            }
        }

        public void TestDFS()
        {
            var graph = new Graph();
            var rootNode = graph.Nodes[0];
            DFSIterative(graph, rootNode);

        }
    }
}