using System;
using System.Collections.Generic;
using System.Linq;

namespace dot_core_asp.Models
{
    public class FloodFill
    {
        // Flood fill algo area can be modelled as a graph traversal area problem represented as a matrix, where each element of the matrix is a vertex
        // that has nearest neighbours in any direction. For example with diagonals this implies 8 nearest neighbours for any vertex that is not a boundary
        // point.

        /// <summary>
        /// Let x,y be starting point and let n, m indicate boundaries
        /// </summary>
        public void FloodFillDFS(int x, int y, Dictionary<(int, int), bool> visited, int n, int m)
        {
            // PrintVisited(visited);
            if (x >= n | y >= m)
                return;
            else if (x < 0 | y < 0)
                return;
            else if (visited[(x,y)])
                return;

            visited[(x,y)] = true;
            PrintVisited(visited);

            FloodFillDFS(x-1, y-1, visited, n, m);
            FloodFillDFS(x-1, y, visited, n, m);
            FloodFillDFS(x-1, y+1, visited, n, m);
            FloodFillDFS(x, y-1, visited, n, m);
            FloodFillDFS(x, y+1, visited, n, m);
            FloodFillDFS(x+1, y-1, visited, n, m);
            FloodFillDFS(x+1, y, visited, n, m);
            FloodFillDFS(x+1, y+1, visited, n, m);
        }

        public static void PrintVisited(Dictionary<(int,int), bool> visited)
        {
            Console.WriteLine(String.Join("; ", visited.Where(ele => ele.Value).Select(ele => String.Format("{0}",ele.Key))));
        }

        public void TestFloodFillDFS()
        {
            var visited = new Dictionary<(int, int), bool>();
            var keyRange = Enumerable.Range(0, 3);
            (int, int) key;
            foreach (var ite in keyRange)
            {
                foreach (var jte in keyRange)
                {
                    key = (ite, jte);
                    visited[key] = false;
                }
            }
            // visited.ToDictionary(p => p.Key, p => false);
            FloodFillDFS(0,0, visited, 3, 3);
        }
    }
}