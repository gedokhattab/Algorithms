using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Template_LCS
{
    class Program
    {
        static void Main(string[] args)
        {
            if(!RunTests("input.txt", 35))
            {
                return;
            }
            else
            {
                Console.WriteLine("\nCongratulations... your program runs successfully");
            }
          
        }

        private static bool RunTests(string filePath, int timeLimit)
        {
            long nCases;
            long result, actualResult;
            StreamReader sr;
            TextReader origConsole = Console.In;
            sr = new StreamReader(filePath);
            Console.SetIn(sr);
            
            int s1length, s2length;
            string [] s1, s2;

            nCases = long.Parse(Console.ReadLine());

            for (long i = 0; i < nCases; i++)
            {

                s1length = int.Parse(Console.ReadLine());
                s1 = Console.ReadLine().Split(' ');

                s2length = int.Parse(Console.ReadLine());
                s2 = Console.ReadLine().Split(' ');

                long timeBefore = System.Environment.TickCount;
                result = LCS.MaximumCommonWords(s1, s2, s1length, s2length);
                long timeAfter = System.Environment.TickCount;


                if (timeAfter - timeBefore > timeLimit)
                {
                    Console.WriteLine("Time limit exceed: case # " + (i + 1));
                    Console.WriteLine("diff time =" + (timeAfter - timeBefore));
                    sr.Close();
                    return false;
                }

                actualResult = long.Parse(Console.ReadLine());
                if (actualResult != result)
                {
                    Console.WriteLine("Wrong Answer: case # " + (i + 1) + ": your answer = "
                        + result + ", correct answer = " + actualResult);
                    sr.Close();
                    return false;
                }
                Console.WriteLine("Case # " + (i + 1) + ": your answer = "
                        + result + ", correct answer = " + actualResult);
               

                sr.ReadLine(); sr.ReadLine(); sr.ReadLine();
            }
            sr.Close();

            Console.SetIn(origConsole);
            return true;
        }
    }
}
