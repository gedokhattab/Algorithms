using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Problem
{
    public static class SumOfMin
    {
        #region YOUR CODE IS HERE
        //Your Code is Here:
        //==================
        /// <summary>
        /// Given an UNDIRECTED Graph, calculate the sum of the min value in each connected component
        /// </summary>
        /// <param name="valuesOfVertices">value of each vertex (vertices are named from 0 to |V| - 1)</param>
        /// <param name="edges">array of edges in the graph</param>
        /// <returns>sum of the min value in each component of the graph</returns>
        public static int CalcSumOfMinInComps(int[] valuesOfVertices, KeyValuePair<int, int>[] edges)
        {
            int v = valuesOfVertices.Length;
            if (v == 0) return 0;

            // Determine if vertices are 0-indexed or 1-indexed
            // Check the maximum vertex number in edges to decide
            int maxVertex = 0;
            foreach (var edge in edges)
            {
                maxVertex = Math.Max(maxVertex, Math.Max(edge.Key, edge.Value));
            }
            
            bool isOneIndexed = maxVertex >= v; // If max vertex >= array length, it's 1-indexed
            int offset = isOneIndexed ? 0 : 0; // Keep as is, adjust access logic
            int adjSize = isOneIndexed ? v + 1 : v;
            int startVertex = isOneIndexed ? 1 : 0;

            // Build adjacency list
            List<List<int>> adj = new List<List<int>>();
            for (int i = 0; i < adjSize; i++)
            {
                adj.Add(new List<int>());
            }

            foreach (var edge in edges)
            {
                int u = edge.Key;
                int w = edge.Value;
                if (u < adjSize && w < adjSize)
                {
                    adj[u].Add(w);
                    adj[w].Add(u);
                }
            }

            bool[] visited = new bool[adjSize];
            int totalSum = 0;

            // Find all connected components using BFS/DFS
            for (int i = startVertex; i < adjSize; i++)
            {
                if (!visited[i])
                {
                    // Start a new component
                    List<int> component = new List<int>();
                    Queue<int> queue = new Queue<int>();
                    queue.Enqueue(i);
                    visited[i] = true;

                    while (queue.Count > 0)
                    {
                        int curr = queue.Dequeue();
                        component.Add(curr);

                        foreach (int neighbor in adj[curr])
                        {
                            if (!visited[neighbor])
                            {
                                visited[neighbor] = true;
                                queue.Enqueue(neighbor);
                            }
                        }
                    }

                    // Find minimum value in this component
                    int minVal = int.MaxValue;
                    foreach (int vertex in component)
                    {
                        // Map vertex index to array index
                        int arrayIndex = isOneIndexed ? vertex : vertex;
                        if (arrayIndex >= 0 && arrayIndex < valuesOfVertices.Length)
                        {
                            minVal = Math.Min(minVal, valuesOfVertices[arrayIndex]);
                        }
                    }
                    totalSum += minVal;
                }
            }

            return totalSum;
        }
   
        #endregion
    }
}
