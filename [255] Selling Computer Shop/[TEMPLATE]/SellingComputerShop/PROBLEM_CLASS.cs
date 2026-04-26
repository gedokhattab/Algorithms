using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Problem
{
    // *****************************************
    // DON'T CHANGE CLASS OR FUNCTION NAME
    // YOU CAN ADD FUNCTIONS IF YOU NEED TO
    // *****************************************
    public static class PROBLEM_CLASS
    {
        #region YOUR CODE IS HERE 

        public enum SOLUTION_TYPE { NAIVE, EFFICIENT };
        public static SOLUTION_TYPE solType = SOLUTION_TYPE.EFFICIENT;

        //Your Code is Here:
        //==================
        /// Write an efficient algorithm to get the maximum profit by selling the computer shop
        /// </summary>
        /// <param name="car1_load">The load car 1 can carry</param>
        /// <param name="car2_load">The load car 2 can carry</param>
        /// <param name="N">The number of items in the shop</param>
        /// <param name="loads">The weights of items exist</param>
        /// <param name="prices">The prices of items exist</param>
        /// <returns>The maximum profit</returns>
        static public int RequiredFunction(int car1_load, int car2_load, int N, int[] loads, int[] prices)
        {
            // 3D DP: dp[w1][w2] = max profit with car1 capacity w1 and car2 capacity w2
            int[,] dp = new int[car1_load + 1, car2_load + 1];

            for (int i = 0; i < N; i++)
            {
                // Process from bottom-right to top-left to avoid using same item twice
                for (int w1 = car1_load; w1 >= 0; w1--)
                {
                    for (int w2 = car2_load; w2 >= 0; w2--)
                    {
                        // Option 1: Don't take item i
                        int best = dp[w1, w2];

                        // Option 2: Put item i in car1 (if it fits)
                        if (w1 >= loads[i])
                        {
                            best = Math.Max(best, dp[w1 - loads[i], w2] + prices[i]);
                        }

                        // Option 3: Put item i in car2 (if it fits)
                        if (w2 >= loads[i])
                        {
                            best = Math.Max(best, dp[w1, w2 - loads[i]] + prices[i]);
                        }

                        dp[w1, w2] = best;
                    }
                }
            }

            return dp[car1_load, car2_load];
        }
        #endregion
    }
}
