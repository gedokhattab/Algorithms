using System;
using System.Collections.Generic;
using System.Linq;

namespace Problem
{
	public static class PROBLEM_CLASS
	{
		public class Landmark
		{
			public int Id;
			public int X, Y;
			public bool IsInside;

			public Landmark(int id, int x, int y, bool isInside)
			{
				Id = id;
				X = x;
				Y = y;
				IsInside = isInside;
			}
        }


        #region YOUR CODE IS HERE
        //Your Code is Here:
        //==================
        /// <summary>
        /// Find the shortest path from "goerge" to any of the landmarks that is outside the Honor Stone 
        /// </summary>
        /// <param name="landmarks">list of Landmarks, each with Id, x, y, IsInside </param>
        /// <param name="trails">list of all trails, each consists of landmark1, landmark2, length</param>
        /// <param name="N">number of landmarks</param>
        /// <returns>value of the shortest path from goerge to outside </returns>
        public static int RequiredFunction(List<Landmark> landmarks, List<Tuple<int, int, int>> trails, int N)
        {
            // Find gorge (Id == 0)
            int gorgeIdx = -1;
            for (int i = 0; i < N; i++)
                if (landmarks[i].Id == 0) { gorgeIdx = i; break; }
            if (gorgeIdx == -1) gorgeIdx = 0;

            Landmark gorge = landmarks[gorgeIdx];

            // Precompute SQUARED distance from gorge for each landmark (O(N))
            // Using squared distances avoids sqrt and keeps values as integers for radix sort
            long[] distSq = new long[N];
            for (int i = 0; i < N; i++)
            {
                long dx = landmarks[i].X - gorge.X;
                long dy = landmarks[i].Y - gorge.Y;
                distSq[i] = dx * dx + dy * dy;
            }

            // Map Id -> index
            Dictionary<int, int> idToIndex = new Dictionary<int, int>();
            for (int i = 0; i < N; i++)
                idToIndex[landmarks[i].Id] = i;

            // Build DIRECTED adjacency list: only edges going FARTHER from gorge
            List<List<Tuple<int, int>>> adj = new List<List<Tuple<int, int>>>(N);
            for (int i = 0; i < N; i++)
                adj.Add(new List<Tuple<int, int>>());

            foreach (var trail in trails)
            {
                if (!idToIndex.ContainsKey(trail.Item1) || !idToIndex.ContainsKey(trail.Item2)) continue;
                int u = idToIndex[trail.Item1];
                int v = idToIndex[trail.Item2];
                int w = trail.Item3;

                // Only allow movement to the farther node (compare squared distances)
                if (distSq[v] > distSq[u])
                    adj[u].Add(new Tuple<int, int>(v, w));
                else if (distSq[u] > distSq[v])
                    adj[v].Add(new Tuple<int, int>(u, w));
                // If equal distances, skip (ambiguous, can't satisfy "strictly farther")
            }

            // O(N) Radix Sort on squared distances
            int[] order = RadixSortIndicesByKey(distSq, N);

            // Shortest path DP
            int[] dp = new int[N];
            for (int i = 0; i < N; i++) dp[i] = int.MaxValue;
            dp[gorgeIdx] = 0;

            // Process in topological order (increasing distance from gorge)
            foreach (int u in order)
            {
                if (dp[u] == int.MaxValue) continue;
                foreach (var edge in adj[u])
                {
                    int v = edge.Item1;
                    int w = edge.Item2;
                    if (dp[u] + w < dp[v])
                        dp[v] = dp[u] + w;
                }
            }

            // Find minimum distance to any outside landmark
            int minDist = int.MaxValue;
            for (int i = 0; i < N; i++)
                if (!landmarks[i].IsInside && dp[i] < minDist)
                    minDist = dp[i];

            return minDist == int.MaxValue ? -1 : minDist;
        }

        // O(N) Radix Sort - sorts indices 0..N-1 by their corresponding key values
        private static int[] RadixSortIndicesByKey(long[] keys, int N)
        {
            int[] order = new int[N];
            for (int i = 0; i < N; i++) order[i] = i;
            
            // Process 8 bits at a time (4 passes for 64-bit values)
            int[] temp = new int[N];
            int[] count = new int[256];
            
            for (int shift = 0; shift < 64; shift += 8)
            {
                // Reset counts
                for (int i = 0; i < 256; i++) count[i] = 0;
                
                // Count occurrences
                for (int i = 0; i < N; i++)
                {
                    int bucket = (int)((keys[order[i]] >> shift) & 0xFF);
                    count[bucket]++;
                }
                
                // Compute prefix sums (starting positions)
                int sum = 0;
                for (int i = 0; i < 256; i++)
                {
                    int c = count[i];
                    count[i] = sum;
                    sum += c;
                }
                
                // Place elements in sorted order
                for (int i = 0; i < N; i++)
                {
                    int bucket = (int)((keys[order[i]] >> shift) & 0xFF);
                    temp[count[bucket]++] = order[i];
                }
                
                // Swap arrays
                int[] swap = order;
                order = temp;
                temp = swap;
            }
            
            return order;
        }
        #endregion
    }

}
