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
            int V = vertices.Count;
            if (V == 0) return -1;

            // Build DIRECTED adjacency list: only edges going downhill (higher -> lower elevation)
            Dictionary<string, List<Tuple<string, int>>> adj = new Dictionary<string, List<Tuple<string, int>>>();
            foreach (var vertex in vertices)
                adj[vertex.Key] = new List<Tuple<string, int>>();

            foreach (var edge in edges)
            {
                string u = edge.Key.Key;
                string v = edge.Key.Value;
                int length = edge.Value;
                
                // Only add directed edges going downhill
                if (vertices[u] > vertices[v])
                    adj[u].Add(new Tuple<string, int>(v, length));
                else if (vertices[v] > vertices[u])
                    adj[v].Add(new Tuple<string, int>(u, length));
                // Skip if same elevation (can't ski)
            }

            // Map vertex names to indices for O(1) array access
            string[] names = new string[V];
            long[] elevations = new long[V];
            int idx = 0;
            Dictionary<string, int> nameToIdx = new Dictionary<string, int>();
            foreach (var kv in vertices)
            {
                names[idx] = kv.Key;
                elevations[idx] = kv.Value;
                nameToIdx[kv.Key] = idx;
                idx++;
            }

            int startIdx = nameToIdx[startVertex];
            int targetIdx = nameToIdx.ContainsKey("T") ? nameToIdx["T"] : -1;
            if (targetIdx == -1) return -1;

            // Convert adjacency list to use indices
            List<List<Tuple<int, int>>> adjIdx = new List<List<Tuple<int, int>>>();
            for (int i = 0; i < V; i++)
                adjIdx.Add(new List<Tuple<int, int>>());

            foreach (var kv in adj)
            {
                int u = nameToIdx[kv.Key];
                foreach (var edge in kv.Value)
                {
                    int v = nameToIdx[edge.Item1];
                    adjIdx[u].Add(new Tuple<int, int>(v, edge.Item2));
                }
            }

            // O(V) Radix Sort by elevation (descending for topological order)
            int[] order = RadixSortByElevationDescending(elevations, V);

            // DP for shortest path in DAG
            int[] dist = new int[V];
            for (int i = 0; i < V; i++) dist[i] = int.MaxValue;
            dist[startIdx] = 0;

            // Process in topological order (descending elevation)
            foreach (int u in order)
            {
                if (dist[u] == int.MaxValue) continue;
                foreach (var edge in adjIdx[u])
                {
                    int v = edge.Item1;
                    int w = edge.Item2;
                    if (dist[u] + w < dist[v])
                        dist[v] = dist[u] + w;
                }
            }

            return dist[targetIdx] == int.MaxValue ? -1 : dist[targetIdx];
        }

        // O(V) Radix Sort - sorts indices by elevation in DESCENDING order
        private static int[] RadixSortByElevationDescending(long[] elevations, int V)
        {
            int[] order = new int[V];
            for (int i = 0; i < V; i++) order[i] = i;

            // For descending order, we negate or reverse the comparison
            // We'll sort ascending then reverse, or use bit manipulation for negatives
            int[] temp = new int[V];
            int[] count = new int[256];

            // Handle negatives: offset by adding large constant or use signed comparison
            // Simple approach: 8 passes on 64-bit values
            for (int shift = 0; shift < 64; shift += 8)
            {
                for (int i = 0; i < 256; i++) count[i] = 0;

                for (int i = 0; i < V; i++)
                {
                    // For descending, flip the bits
                    ulong key = (ulong)elevations[order[i]];
                    int bucket = (int)((key >> shift) & 0xFF);
                    count[bucket]++;
                }

                int sum = 0;
                for (int i = 0; i < 256; i++)
                {
                    int c = count[i];
                    count[i] = sum;
                    sum += c;
                }

                for (int i = 0; i < V; i++)
                {
                    ulong key = (ulong)elevations[order[i]];
                    int bucket = (int)((key >> shift) & 0xFF);
                    temp[count[bucket]++] = order[i];
                }

                int[] swap = order;
                order = temp;
                temp = swap;
            }

            // Reverse for descending order
            Array.Reverse(order);
            return order;
        }
        #endregion
    }
}
