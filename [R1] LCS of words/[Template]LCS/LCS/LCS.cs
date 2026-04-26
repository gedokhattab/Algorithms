using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Template_LCS
{
    // *****************************************
    // DON'T CHANGE CLASS OR FUNCTION NAME
    // YOU CAN ADD FUNCTIONS IF YOU NEED TO
    // *****************************************

    class LCS
    {

        //Your Code is Here:
        //==================
        /// <summary>
        /// given two sentences, find the max number of common subsequence words.
        /// </summary>
        /// <param name="s1">1st sentence</param>
        /// <param name="s2">2nd sentence</param>
        /// <param name="s1length">number of words in 1st sentence</param>
        /// <param name="s2length">number of words in 2nd sentence</param>
        /// <returns>max number of common subsequence words between s1 & s2</returns>
        public static int MaximumCommonWords(string[] s1, string[] s2, int s1length, int s2length)
        {
            // Case-insensitive: normalize all words to lowercase
            string[] w1 = new string[s1length];
            string[] w2 = new string[s2length];
            for (int i = 0; i < s1length; i++) w1[i] = s1[i].ToLower();
            for (int j = 0; j < s2length; j++) w2[j] = s2[j].ToLower();

            // dp[i][j] = LCS length of first i words of s1 and first j words of s2
            int[,] dp = new int[s1length + 1, s2length + 1];

            for (int i = 1; i <= s1length; i++)
            {
                for (int j = 1; j <= s2length; j++)
                {
                    if (w1[i - 1] == w2[j - 1])
                        dp[i, j] = dp[i - 1, j - 1] + 1;       // words match: extend LCS
                    else
                        dp[i, j] = Math.Max(dp[i - 1, j], dp[i, j - 1]); // take best so far
                }
            }

            return dp[s1length, s2length];
        }
    }
}
