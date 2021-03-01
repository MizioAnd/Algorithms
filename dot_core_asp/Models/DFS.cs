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

        public Vertex Bottom()
        {
            return this.StackElements.FirstOrDefault();
        }
    
        public void Pop()
        {
            this.StackElements.RemoveAt(this.StackElements.Count - 1);
        }

        public void PopBottom()
        {
            this.StackElements.RemoveAt(0);
        }
    }

    public class Node
    {
        public Vertex ResidingNode {get; set;}
        public List<Vertex> Children {get; set;}
        public Vertex Parent {get; set;}

        private Vertex child1;
        public Vertex Child1 
        {
            get
            {
                return child1;
            } 
            set
            {
                child1 = value;
                uChildrenDel += this.UpdateChildren;
                uChildrenDel();
                uChildrenDel -= this.UpdateChildren;
            }
        }

        private Vertex child2;
        public Vertex Child2 
        {
            get
            {
                return child2;
            }
            set
            {
                child2 = value;
                uChildrenDel += this.UpdateChildren;
                uChildrenDel();
                uChildrenDel -= this.UpdateChildren;
            }            
        }

        public delegate void UpdateChildrenDelegate();
        // We are using + and - operators, since that enables the same delegate to be used for any future defined function with same method signature.
        // Otherwise we would have to declare a new delegate for every new method, which would be a waste of memory.
        UpdateChildrenDelegate uChildrenDel;

        public Node(List<Vertex> children = null)
        {
            if (children != null)
                Children = children;
            else
                Children = new List<Vertex>(){ Child1, Child2 };
        }

        public void UpdateChildren()
        {
            if (Child1 != Children[0])
            {
                Children.RemoveAt(0);
                Children.Insert(0, Child1);
            }
            else if (Child2 != Children[1])
            {
                Children.RemoveAt(1);
                Children.Insert(1, Child2);                
            }
        }
    }

    public class Graph
    {
        public List<List<Vertex>> NodesNew {get; private set;}
        public List<Node> Nodes {get; set;}
        public List<Vertex> verticesInTree {get; set;}
        public int CountVisitedNode => Nodes.Where(x => x.ResidingNode.Visited).Count();

        public Graph()
        {
            Nodes = new List<Node>();
            NodesNew = new List<List<Vertex>>();
            verticesInTree = new List<Vertex>();
            FillGraph();
        }

        public Node GetNode(Vertex vertex)
        {
            // var node = this.Nodes.Where(x => x.ResidingNode.idx == vertex.idx).FirstOrDefault();
            var node = this.Nodes.Where(x => x.ResidingNode.idxGrid == vertex.idxGrid).FirstOrDefault();
            return node;
        }

        public void PrintVisitedVertices()
        {
            // Console.WriteLine(String.Join("; ", this.Nodes.Where(x => x.ResidingNode.Visited).Select(x => x.ResidingNode.idx).ToList()));
            Console.WriteLine(String.Join("; ", this.Nodes.Where(x => x.ResidingNode.Visited).Select(x => x.ResidingNode.idxGrid).ToList()));
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
            // At 0-th level with j=0 we get,
            // 1, .., 2^(0+1)-1 => 1, .., 1 => 1
            // Create 3 level Graph
            // Each level has 2^(level) nodes
            // At 1'th level there is 2 nodes
            var endLevel = 2;
            var rangeLevels = Enumerable.Range(0, endLevel + 1);
            
            // Create all nodes and collect them in a list
            var countNodes = rangeLevels.Select(x => (int)Math.Pow(2, x)).Sum();
            verticesInTree = new List<Vertex>(countNodes);
            foreach (var idx_vertex in Enumerable.Range(1, countNodes))
            {
                var vertex = new Vertex();
                vertex.idx = idx_vertex;
                vertex.idxGrid = (idx_vertex,0);
                verticesInTree.Add(vertex);
            }

            IEnumerable<int> rangeNodeIndices;
            IEnumerable<int> rangeNodeIndicesParent;
            IEnumerable<int> rangeNodeIndicesChild;
            foreach (var level in rangeLevels)
            {   
                
                if (level == 0)
                {
                    rangeNodeIndicesChild = CreateNodeIndices(level + 1);
                    var node = new Node();
                    node.ResidingNode = verticesInTree[level];
                    node.Child1 = verticesInTree[rangeNodeIndicesChild.ElementAt(0)];
                    node.Child2 = verticesInTree[rangeNodeIndicesChild.ElementAt(0) - 1];

                    Nodes.Add(node);
                }
                else
                {
                    rangeNodeIndicesParent = CreateNodeIndices(level - 1);
                    rangeNodeIndices = CreateNodeIndices(level);
                    rangeNodeIndicesChild = CreateNodeIndices(level + 1);

                    var iteSlow = 0;
                    var ite = 0;
                    bool changeParentNode;
                    foreach (var jte in Enumerable.Range(0, rangeNodeIndices.Count()))
                    {
                        var idx_current = rangeNodeIndices.ElementAt(jte);
                        var idx_child = rangeNodeIndicesChild.ElementAt(ite);
                        var idx_parent = rangeNodeIndicesParent.ElementAt(iteSlow);
                        var node = new Node();

                        node.ResidingNode = verticesInTree[idx_current - 1];

                        if (level != endLevel)
                        {
                            node.Child1 = verticesInTree[idx_child - 1];
                            node.Child2 = verticesInTree[idx_child];
                        }
                        node.Parent = verticesInTree[idx_parent - 1];

                        Nodes.Add(node);
                        ite += 2;
                        changeParentNode = (jte + 1) % 2 == 0;
                        if (changeParentNode) { iteSlow += 1; }
                    }
                }              
            }
        }

        public IEnumerable<int> CreateNodeIndices(int level)
        {
            var lowestNodeIndexInLevel = (int)Math.Pow(2, level);
            var nodesInLevel = lowestNodeIndexInLevel;
            var rangeNodeIndices = Enumerable.Range(lowestNodeIndexInLevel, nodesInLevel);
            return rangeNodeIndices;

        }
    }

    public class Vertex
    {   
        public bool Visited {get; set;} = false;
        public int idx {get; set;}
        public (int, int) idxGrid {get; set;}
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

            G.PrintVisitedVertices();
            while (StackElements.Count != 0)
            {
                var v = StackElements.Top();
                var nodeOfv = G.GetNode(v);
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

                G.PrintVisitedVertices();

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