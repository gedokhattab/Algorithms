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
            int vertexCount = vertices.Count;
            if (vertexCount == 0) return -1;
            if (!vertices.ContainsKey("T")) return -1;

            var adjList = new Dictionary<string, List<(string neighbor, int length)>>();
            foreach (var vertex in vertices)
                adjList[vertex.Key] = new List<(string, int)>();

            foreach (var edge in edges)
            {
                string u = edge.Key.Key;
                string v = edge.Key.Value;
                int length = edge.Value;

                if (vertices[u] > vertices[v])
                    adjList[u].Add((v, length));
                else if (vertices[v] > vertices[u])
                    adjList[v].Add((u, length));
            }

            var visited = new HashSet<string>();
            var topoOrder = new Stack<string>();

            void DFS(string node)
            {
                visited.Add(node);
                foreach (var (neighbor, length) in adjList[node])
                    if (!visited.Contains(neighbor))
                        DFS(neighbor);
                topoOrder.Push(node);
            }

            foreach (var vertex in vertices)
                if (!visited.Contains(vertex.Key))
                    DFS(vertex.Key);

            var shortestDistance = new Dictionary<string, int>();
            foreach (var vertex in vertices)
                shortestDistance[vertex.Key] = int.MaxValue;
            shortestDistance[startVertex] = 0;

            foreach (string u in topoOrder)
            {
                if (shortestDistance[u] == int.MaxValue) continue;

                foreach (var (neighbor, length) in adjList[u])
                {
                    int candidate = shortestDistance[u] + length;
                    if (candidate < shortestDistance[neighbor])
                        shortestDistance[neighbor] = candidate;
                }
            }

            return shortestDistance["T"] == int.MaxValue ? -1 : shortestDistance["T"];
        }
        #endregion
    }
}
