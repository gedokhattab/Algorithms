using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Problem
{
    public static class PROBLEM_CLASS
    {
        #region YOUR CODE IS HERE
        //Your Code is Here:
        //==================
        /// <summary>
        /// Ali Baba decides to go on a skating travel in the alpine mountain. He has stolen a pair of skis and a trail map listing 
        /// the mountain’s surfaces and slopes (n in total), and he wants to ski from surface S to surface T where a treasure is exists. 
        /// </summary>
        /// <param name="vertices">array of surfaces and their elevations </param>
        /// <param name="edges">array of trails and their lengths </param>
        /// <param name="startVertex">name of the start surface to begin from it</param>
        /// <returns>the minimum valid distance from source “S” to target “T”.</returns>
        public static int RequiredFunction(Dictionary<string, int> vertices, Dictionary<KeyValuePair<string, string>, int> edges, string startVertex)
        {
            // Build adjacency list
            Dictionary<string, List<Tuple<string, int>>> adj = new Dictionary<string, List<Tuple<string, int>>>();
            foreach (var vertex in vertices)
            {
                adj[vertex.Key] = new List<Tuple<string, int>>();
            }

            foreach (var edge in edges)
            {
                string u = edge.Key.Key;
                string v = edge.Key.Value;
                int length = edge.Value;
                adj[u].Add(new Tuple<string, int>(v, length));
                adj[v].Add(new Tuple<string, int>(u, length));
            }

            // Dijkstra's algorithm with elevation constraint (can only go downhill)
            Dictionary<string, int> dist = new Dictionary<string, int>();
            foreach (var vertex in vertices)
            {
                dist[vertex.Key] = int.MaxValue;
            }
            dist[startVertex] = 0;

            // Priority queue: (distance, vertex)
            SortedSet<Tuple<int, string>> pq = new SortedSet<Tuple<int, string>>(Comparer<Tuple<int, string>>.Create((a, b) => {
                if (a.Item1 != b.Item1) return a.Item1.CompareTo(b.Item1);
                return a.Item2.CompareTo(b.Item2);
            }));
            pq.Add(new Tuple<int, string>(0, startVertex));

            while (pq.Count > 0)
            {
                var current = pq.Min;
                pq.Remove(current);
                int d = current.Item1;
                string u = current.Item2;

                if (d > dist[u]) continue;

                // Check if we reached target T
                if (u == "T")
                {
                    return d;
                }

                foreach (var neighbor in adj[u])
                {
                    string v = neighbor.Item1;
                    int weight = neighbor.Item2;

                    // Elevation constraint: can only go from higher to lower elevation
                    if (vertices[u] > vertices[v])
                    {
                        if (dist[u] + weight < dist[v])
                        {
                            dist[v] = dist[u] + weight;
                            pq.Add(new Tuple<int, string>(dist[v], v));
                        }
                    }
                }
            }

            return -1; // No valid path found
        }
        #endregion
    }
}
