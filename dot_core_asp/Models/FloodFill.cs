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

        public bool GraphTraversalBlockedSpacesAnd4MovesDFS(int x, int y, 
        Dictionary<(int, int), bool> visited, Dictionary<(int, int), bool> isBlockedSpace, int n, int m, int destX, int destY)
        {
            // Algorithms that works when traversing a maze.
            // PrintVisited(visited);
            if (x == destX & y == destY)
                return true;
            else if (x >= n | y >= m)
                return false;
            else if (x < 0 | y < 0)
                return false;
            else if (isBlockedSpace[(x,y)])
                return false;
            else if (visited[(x,y)])
                return false;

            visited[(x,y)] = true;
            PrintVisited(visited);

            if (GraphTraversalBlockedSpacesAnd4MovesDFS(x-1, y, visited, isBlockedSpace, n, m, destX, destY))
                return true;
            else if (GraphTraversalBlockedSpacesAnd4MovesDFS(x, y-1, visited, isBlockedSpace, n, m, destX, destY))
                return true;
            else if (GraphTraversalBlockedSpacesAnd4MovesDFS(x, y+1, visited, isBlockedSpace, n, m, destX, destY))
                return true;
            else if (GraphTraversalBlockedSpacesAnd4MovesDFS(x+1, y, visited, isBlockedSpace, n, m, destX, destY))
                return true;
            else
                return false;
        }

        public bool ShortestPathTraversal(int x, int y, 
        Dictionary<(int, int), bool> visited, Dictionary<(int, int), bool> isBlockedSpace, Dictionary<(int, int), int> distToDestination, int n, int m, int destX, int destY, (int, int) start = default((int, int)))
        {
            // Algorithms that works when traversing a maze and selects the shortest path
            // In every step it should update distance to destination for all visited block
            // Distance is defined by allowed moves and in this case it is the absolute distance to source (x0, y0), dist(x,y) = |x-x0| + |y-y0|
            // PrintVisited(visited);

            // bool isNewStep;

            if (x == destX & y == destY)
            {
                visited[(x,y)] = true;
                ComputeAllDistancesToDestinationAndCompareNeighbourDist(distToDestination, (x, y), start);
                PrintVisited(visited);
                PrintDistances(distToDestination);
                return true;
            }
            else if (x >= n | y >= m)
                return false;
            else if (x < 0 | y < 0)
                return false;
            else if (isBlockedSpace[(x,y)])
                return false;
            else if (visited[(x,y)])
                return false;

            // compute distance to destination and insert value in dictionary
            ComputeAllDistancesToDestinationAndCompareNeighbourDist(distToDestination, (x, y), (0, 0));
            // if (!ComputeAllDistancesToDestinationAndCompareNeighbourDist(distToDestination, (x, y), (destX, destY)) & visited[(x,y)])
            // if (ComputeAllDistancesToDestinationAndCompareNeighbourDist(distToDestination, (x, y), start))
                // return false;

            visited[(x,y)] = true;
            PrintVisited(visited);
            PrintDistances(distToDestination);

            if (ShortestPathTraversal(x-1, y, visited, isBlockedSpace, distToDestination, n, m, destX, destY))
                return true;
            else if (ShortestPathTraversal(x, y-1, visited, isBlockedSpace, distToDestination, n, m, destX, destY))
                return true;
            else if (ShortestPathTraversal(x, y+1, visited, isBlockedSpace, distToDestination, n, m, destX, destY))
                return true;
            else if (ShortestPathTraversal(x+1, y, visited, isBlockedSpace, distToDestination, n, m, destX, destY))
                return true;
            else
                return false;
        }

        private bool NeighbourDistanceIsShorter(Dictionary<(int, int), int> distances, (int, int) x_y, int distance_to_source)
        {
            var nearestNeighbours = new List<(int,int)>(){ (x_y.Item1-1 , x_y.Item2),(x_y.Item1 , x_y.Item2-1),(x_y.Item1+1 , x_y.Item2),(x_y.Item1 , x_y.Item2+1) };
            foreach (var neighbour in nearestNeighbours)
            {
                if (distances.Keys.Contains(neighbour) && distance_to_source < distances[neighbour])
                    return true;
            }
            return false;
        }

        public int DistanceToDestination((int, int) x_y, (int,int) x0_y0)
        {
            return Math.Abs(x0_y0.Item1 - x_y.Item1) + Math.Abs(x0_y0.Item2 - x_y.Item2);
        }

        public int DistanceFromStartToCurrentCord((int, int) x_y, (int,int) x0_y0)
        {
            return Math.Abs(x0_y0.Item1 - x_y.Item1) + Math.Abs(x0_y0.Item2 - x_y.Item2);
        }

        /// <summary>
        /// Reassures that distance from start to next position is longer than from start to current position.
        /// </summary>
        /// <returns></returns>
        public bool ComputeAllDistancesToDestinationAndCompareNeighbourDist(Dictionary<(int, int), int> distances, (int, int) x_y, (int,int) x0_y0)
        {
            // Todo: we cannot use absolute distance since it has to be stepwise incrementing distance instead
            var currentDistance = distances.Keys.Count;
            var dist = currentDistance + 1;
            // var dist = DistanceToDestination(x_y, x0_y0);
            if (NeighbourDistanceIsShorter(distances, x_y, dist))
            {
                return true;
            }                
            else
            {
                // Compute new distance
                distances[x_y] = dist;
                return false;
            }

            // Recompute other distances
            // foreach (var key in distances.Keys)
            // {
            //     distances[key] = DistanceToDestination(key, x0_y0);
            // }
        }

        public int SelectShortestPath(Dictionary<(int, int), bool> visited, out List<(int, int)> shortestPath, (int, int) start, (int, int) end)
        {
            // Consider all connected step combinations possible corresponding to allowed moves and avoiding any blocked space

            // Simple correction of path. Subtract one step for every four-connected steps taken.
            var shortestPathDict = visited.ToDictionary(entry => entry.Key, entry => entry.Value);
            var positionWithClosedCircle = new List<(int, int)>();
            var closedCircles = 0;
            var dim = (int)Math.Sqrt(visited.Keys.Count);
            var range = Enumerable.Range(0, dim-1);

            foreach (var key1 in range)
            {
                foreach(var key2 in range)
                {
                    if (visited[(key1, key2)] && visited[(key1+1, key2)] && visited[(key1+1, key2+1)] && visited[(key1, key2+1)])
                    {
                        closedCircles += 1;
                        positionWithClosedCircle.Add((key1, key2));
                        // shortestPathDict[(key1, key2+1)] = false;
                        // Or
                        // shortestPathDict[(key1+1, key2)] = false;
                    }
                }
            }

            // Examine all combinations for circle breaking.
            var circleRemovalCombs = CircleRemovalCombinations(closedCircles, positionWithClosedCircle);

            // Todo: foreach of the circle removal combinations a shortest path needs to be computed.
            shortestPath = FindShortestPath(shortestPathDict, circleRemovalCombs, start, end);

            return closedCircles;
        }

        private List<(int, int)> FindShortestPath(Dictionary<(int, int), bool> shortestPathDict, IList<IList<(int, int)>> circleRemovalCombs, (int, int) start, (int, int) end)
        {
            // Todo: foreach of the circle removal combinations a shortest path needs to be computed.
            // var shortestPathAfterRemovalOfCircles = shortestPathDict.ToDictionary(entry => entry.Key, entry => entry.Value);
            var shortestPathFinal = new Dictionary<(int, int), bool>();
            var pathLenghtFinal = 0;
            var nearestNeighboursFinal = new Dictionary<(int, int), int>();
            foreach (var removalCombList in circleRemovalCombs)
            {
                var shortestPathAfterRemovalOfCircles = shortestPathDict.ToDictionary(entry => entry.Key, entry => entry.Value);
                foreach (var removalComb in removalCombList)
                {
                    shortestPathAfterRemovalOfCircles[removalComb] = false;
                }
                var nearestNeighbours = CountNearestNeighboursHorizontalVertical(shortestPathAfterRemovalOfCircles);
                var nearestNeighboursLessThanTwo = nearestNeighbours.Where(x => x.Value < 2 && x.Key != start && x.Key != end).Select(x => x.Key);

                foreach (var outlier in nearestNeighboursLessThanTwo)
                    shortestPathAfterRemovalOfCircles[outlier] = false;

                var pathLenght = shortestPathAfterRemovalOfCircles.Where(x => x.Value).Count();
                
                if (pathLenghtFinal == 0 || pathLenght < pathLenghtFinal)
                {
                    pathLenghtFinal = pathLenght;
                    shortestPathFinal = shortestPathAfterRemovalOfCircles;
                    nearestNeighboursFinal = nearestNeighbours;
                }
            }

            // var shortestPathDictMod = shortestPathFinal.ToDictionary(entry => entry.Key, entry => entry.Value);
            // var nearestNeighbours = CountNearestNeighboursHorizontalVertical(shortestPathDict);
            // var nearestNeighboursLessThanTwo = nearestNeighbours.Where(x => x.Value < 2 && x.Key != start && x.Key != end).Select(x => x.Key);

            // foreach (var outlier in nearestNeighboursLessThanTwo)
            //     shortestPathDictMod[outlier] = false;

            Console.WriteLine(String.Format("Nearest neighbours:{0}", String.Join(";", nearestNeighboursFinal.Values.ToList())));
            return shortestPathFinal.Where(x => x.Value).Select(x => x.Key).ToList();
        }

        private IList<IList<(int, int)>> CircleRemovalCombinations(int closedCircles, List<(int, int)> positionWithClosedCircle)
        {
            // var combinations = Math.Pow(2, closedCircles);
            var circleRemovalCombs = new List<IList<(int, int)>>();

            // Todo: create nested loops corresponding to combinations
            foreach (var circleCoords in positionWithClosedCircle)
            {
                if (circleRemovalCombs.Count == 0)
                {
                    circleRemovalCombs.Add(new List<(int, int)>{(circleCoords.Item1, circleCoords.Item2+1)});
                    // circleRemovalCombs.Add(new List<(int,int)>{(circleCoords.Item1+1, circleCoords.Item2)});
                }
                else
                {
                    // Todo: circleRemovalCombs is modified and cannot figurate in foreach.
                    foreach (var listIdx in Enumerable.Range(0, circleRemovalCombs.Count))
                    {
                        var list = circleRemovalCombs[listIdx];
                        var helplist = new List<(int, int)>(list);
                        // helplist.Add((circleCoords.Item1, circleCoords.Item2+1));
                        // list.Add((circleCoords.Item1+1, circleCoords.Item2));
                        list.Add((circleCoords.Item1, circleCoords.Item2+1));
                        // circleRemovalCombs.Add(helplist);
                    }
                }
            }
            return circleRemovalCombs;
        }

        private Dictionary<(int, int), int> CountNearestNeighboursHorizontalVertical(Dictionary<(int, int), bool> shortestPathDict)
        {
            var dim = (int)Math.Sqrt(shortestPathDict.Keys.Count);
            var counter = 0;
            var nearestNeighbours = new Dictionary<(int, int), int>();

            foreach (var key in shortestPathDict.Keys)
            {
                counter = 0;
                var key1 = key.Item1;
                var key2 = key.Item2;
                if (key1+1 < dim && shortestPathDict[(key1+1,key2)])
                    counter += 1;
                if (key1-1 >= 0 && shortestPathDict[(key1-1,key2)])
                    counter += 1;
                if (key2+1 < dim && shortestPathDict[(key1,key2+1)])
                    counter += 1;
                if (key2-1 >= 0 && shortestPathDict[(key1,key2-1)])
                    counter += 1;                                
                
                nearestNeighbours[key] = counter;
            }
            return nearestNeighbours;
        }

        // private bool IsBoundary((int, int) key, int dim)
        // {
        //     if (key.Item1 == 0 || key.Item2 == 0 || key.Item1 == dim-1 || key.Item2 == dim-1)
        //         return true;
        //     else
        //         return false;
        // }

        public static void PrintVisited(Dictionary<(int,int), bool> visited)
        {
            Console.WriteLine(String.Join("; ", visited.Where(ele => ele.Value).Select(ele => String.Format("{0}",ele.Key))));
        }

        public static void PrintDistances(Dictionary<(int,int), int> distances)
        {
            Console.WriteLine(String.Join("; ", distances.Select(ele => String.Format("{0}",ele.Value))));
        }


        public void TestShortestPathTraversal()
        {
            // The maze space is visualized like with blocked indices in (2,0), (1,1)
            // 0 0 0 
            // 0 X 0
            // x s 0
            var start = (0, 0);
            var end = (2,1);
            var visited = new Dictionary<(int, int), bool>();
            var distToDestination = new Dictionary<(int, int), int>();
            var isBlockedSpace = new Dictionary<(int, int), bool>();
            // todo: not able to arrive at source point even though a path does exist.
            // var blockedIndices = new List<(int, int)>(){  };
            var blockedIndices = new List<(int, int)>(){ (1,1), (2,0), (1,2), (2,2), (3,2) };
            // var blockedIndices = new List<(int, int)>(){ (1,1), (0,2) };
            var dim = 5;
            var keyRange = Enumerable.Range(0, dim);
            (int, int) key;
            foreach (var ite in keyRange)
            {
                foreach (var jte in keyRange)
                {
                    key = (ite, jte);
                    visited[key] = false;
    
                    if (blockedIndices.Contains((ite,jte)))
                        isBlockedSpace[key] = true;
                    else
                        isBlockedSpace[key] = false;
                }
            }
            var hasArrivedAtDest = ShortestPathTraversal(start.Item1, start.Item2, visited, isBlockedSpace, distToDestination, dim, dim, end.Item1, end.Item2);   
            var shortestPath = new List<(int,int)>();
            var closedCircles = SelectShortestPath(visited, out shortestPath, start, end);         
            Console.WriteLine(String.Format("Closed circles:{0}", closedCircles));
            Console.WriteLine(String.Format("Steps shortest path:{0}", visited.Where(x => x.Value).Count() - closedCircles));
            Console.WriteLine(String.Format("Shortest path:{0}", String.Join(";", shortestPath)));
            Console.WriteLine(String.Format("Steps shortest path:{0}", shortestPath.Count));
            Console.WriteLine(String.Format("Arrived:{0}", hasArrivedAtDest));
        }

        public void TestGraphTraversalBlockedSpacesAnd4MovesDFS()
        {
            // The maze space is visualized like with blocked indices in (2,0), (1,1)
            // 0 0 0 
            // 0 X 0
            // X 0 0

            var visited = new Dictionary<(int, int), bool>();
            var isBlockedSpace = new Dictionary<(int, int), bool>();
            var blockedIndices = new List<(int, int)>(){ (2,0), (1,1) };
            var dim = 3;
            var keyRange = Enumerable.Range(0, dim);
            (int, int) key;
            foreach (var ite in keyRange)
            {
                foreach (var jte in keyRange)
                {
                    key = (ite, jte);
                    visited[key] = false;
    
                    if (blockedIndices.Contains((ite,jte)))
                        isBlockedSpace[key] = true;
                    else
                        isBlockedSpace[key] = false;
                }
            }
            var hasArrivedAtDest = GraphTraversalBlockedSpacesAnd4MovesDFS(0,0, visited, isBlockedSpace, dim, dim, 2, 1);            
            Console.WriteLine(String.Format("Arrived:{0}", hasArrivedAtDest));
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