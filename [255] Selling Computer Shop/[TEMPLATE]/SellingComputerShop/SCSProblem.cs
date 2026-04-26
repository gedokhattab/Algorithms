using Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Problem
{

    public class Problem : ProblemBase, IProblem
    {
        #region ProblemBase Methods
        public override string ProblemName { get { return "SellingComputerShop"; } }

        public override void TryMyCode()
        {

            int N, car1_load, car2_load;
            int expected, output;

            // 1.
            N = 4;
            car1_load = 5;
            car2_load = 2;
            int[] loads = { 1, 2, 3, 5 };
            int[] prices = { 20, 10, 15, 25 };
            expected = 45;
            //List<int> expected_car1_items = new List<int>() { 1, 2};
            //List<int> expected_car2_items = new List<int>() { 0 };
            output = PROBLEM_CLASS.RequiredFunction(car1_load, car2_load, N, loads, prices);
            PrintCase(car1_load, car2_load, N, loads, prices, output, expected);

            // 2.
            N = 5;
            car1_load = 7;
            car2_load = 10;
            loads = new int[] { 7, 3, 4, 5, 3 };
            prices = new int[] { 42, 12, 40, 25, 40 };
            expected = 134;
            //expected_car1_items = new List<int>() { 0 };
            //expected_car2_items = new List<int>() { 1, 2, 4 };
            output = PROBLEM_CLASS.RequiredFunction(car1_load, car2_load, N, loads, prices);
            PrintCase(car1_load, car2_load, N, loads, prices, output, expected);

            // 3.
            N = 6;
            car1_load = 6;
            car2_load = 9;
            loads = new int[] { 2, 5, 9, 6, 5, 4 };
            prices = new int[] { 10, 12, 30, 15, 20, 25 };
            expected = 65;
            //expected_car1_items = new List<int>() { 0 };
            //expected_car2_items = new List<int>() { 1, 2, 4 };
            output = PROBLEM_CLASS.RequiredFunction(car1_load, car2_load, N, loads, prices);
            PrintCase(car1_load, car2_load, N, loads, prices, output, expected);

            // 4.
            N = 5;
            car1_load = 5;
            car2_load = 7;
            loads = new int[] { 2, 5, 3, 6, 4 };
            prices = new int[] { 10, 20, 15, 18, 12 };
            expected = 47;
            //expected_car1_items = new List<int>() { 0 };
            //expected_car2_items = new List<int>() { 1, 2, 4 };
            output = PROBLEM_CLASS.RequiredFunction(car1_load, car2_load, N, loads, prices);
            PrintCase(car1_load, car2_load, N, loads, prices, output, expected);

            // 5.
            N = 7;
            car1_load = 10;
            car2_load = 8;
            loads = new int[] { 2, 3, 5, 5, 9, 9, 7 };
            prices = new int[] { 15, 18, 25, 20, 28, 30, 35 };
            expected = 93;
            //expected_car1_items = new List<int>() { 0 };
            //expected_car2_items = new List<int>() { 1, 2, 4 };
            output = PROBLEM_CLASS.RequiredFunction(car1_load, car2_load, N, loads, prices);
            PrintCase(car1_load, car2_load, N, loads, prices, output, expected);
        }

        Thread tstCaseThr;
        bool caseTimedOut;
        bool caseException;

        protected override void RunOnSpecificFile(string fileName, HardniessLevel level, int timeOutInMillisec)
        {
            /* READ THE TEST CASES FROM THE SPECIFIED FILE, FOR EACH CASE DO:
             * 1) READ ITS INPUT & EXPECTED OUTPUT
             * 2) READ ITS EXPECTED TIMEOUT LIMIT (IF ANY)
             * 3) CALL THE FUNCTION ON THE GIVEN INPUT USING THREAD WITH THE GIVEN TIMEOUT 
             * 4) CHECK THE OUTPUT WITH THE EXPECTED ONE
             */

            int testCases;
            int N = 0;
            int car1_load = 0;
            int car2_load = 0;
            int[] loads = null;
            int[] prices = null;
            int output, actualResult;

            Stream s = new FileStream(fileName, FileMode.Open);
            BinaryReader br = new BinaryReader(s);

            testCases = br.ReadInt32();

            int totalCases = testCases;
            int correctCases = 0;
            int wrongCases = 0;
            int timeLimitCases = 0;
            bool readTimeFromFile = false;
            if (timeOutInMillisec == -1)
            {
                readTimeFromFile = true;
            }
            int i = 1;
            while (testCases-- > 0)
            {
                N = br.ReadInt32();
                car1_load = br.ReadInt32();
                car2_load = br.ReadInt32();
                loads = new int[N];
                prices = new int[N];
                for (int j = 0; j < N; j++)
                {
                    loads[j] = br.ReadInt32();
                }
                for (int j = 0; j < N; j++)
                {
                    prices[j] = br.ReadInt32();
                }
                actualResult = br.ReadInt32();
                output = 0;
                caseTimedOut = true;
                caseException = false;
                Stopwatch sw = null;
                {
                    tstCaseThr = new Thread(() =>
                    {
                        try
                        {
                            sw = Stopwatch.StartNew();
                            output = PROBLEM_CLASS.RequiredFunction(car1_load, car2_load, N, loads, prices);
                            sw.Stop();
                        }
                        catch
                        {
                            caseException = true;
                            output = int.MinValue;
                        }
                        caseTimedOut = false;
                    });

                    if (readTimeFromFile)
                    {
                        timeOutInMillisec = br.ReadInt32();
                    }
                    /*LARGE TIMEOUT FOR SAMPLE CASES TO ENSURE CORRECTNESS ONLY*/
                    if (level == HardniessLevel.Easy)
                    {
                        timeOutInMillisec = 10000; //Large Value 
                    }
                    /*=========================================================*/
                    tstCaseThr.Start();
                    tstCaseThr.Join(timeOutInMillisec);
                }
                Console.WriteLine($"CASE#{i}: N = {N}, car1_load = {car1_load}, car2_load = {car2_load}, result = {output}, expected = {actualResult}, time in ms = {sw.ElapsedMilliseconds}, timeout = {timeOutInMillisec}");

                if (caseTimedOut)       //Timedout
                {
                    Console.WriteLine("Time Limit Exceeded in Case {0}.", i);
                    tstCaseThr.Abort();
                    timeLimitCases++;
                }
                else if (caseException) //Exception 
                {
                    Console.WriteLine("Exception in Case {0}.", i);
                    wrongCases++;
                }
                else if (output == actualResult)    //Passed
                {
                    Console.WriteLine("Test Case {0} Passed!", i);
                    correctCases++;
                }
                else                    //WrongAnswer
                {
                    Console.WriteLine("Wrong Answer in Case {0}.", i);
                    Console.WriteLine(" your answer = " + output + ", correct answer = " + actualResult);
                    wrongCases++;
                }

                i++;
            }
            br.Close();
            s.Close();
            Console.WriteLine();
            Console.WriteLine("# correct = {0}", correctCases);
            Console.WriteLine("# time limit = {0}", timeLimitCases);
            Console.WriteLine("# wrong = {0}", wrongCases);
            Console.WriteLine("\nFINAL EVALUATION (%) = {0}", Math.Round((float)correctCases / totalCases * 100, 0));
        }

        protected override void OnTimeOut(DateTime signalTime)
        {
        }

        /// <summary>
        /// Generate a file of test cases according to the specified params
        /// </summary>
        /// <param name="level">Easy or Hard</param>
        /// <param name="numOfCases">Required number of cases</param>
        /// <param name="includeTimeInFile">specify whether to include the expected time for each case in the file or not</param>
        /// <param name="timeFactor">factor to be multiplied by the actual time</param>
        public override void GenerateTestCases(HardniessLevel level, int numOfCases, bool includeTimeInFile = false, float timeFactor = 1)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Helper Methods

        //private static void PrintCase(int N,int[] loads,int[] prices,int output,int expected,List<int> car1_items,List<int> expected_car1_items,List<int> car2_items,List<int> expected_car2_items)
        private static void PrintCase(int car1_load, int car2_load, int N, int[] loads, int[] prices, int output, int expected)
        {
            /* PRINT THE FOLLOWING
             * 1) INPUT
             * 2) EXPECTED OUTPUT
             * 3) RETURNED OUTPUT
             * 4) WHETHER IT'S CORRECT OR WRONG
             * */
            Console.WriteLine("Car 1 Load: {0}\nCar 2 Load: {1}\nN: {2}", car1_load, car2_load, N);

            Console.Write("Loads = [");
            for (int i = 0; i < loads.Length; i++)
            {
                Console.Write(loads[i] + " ");
            }
            Console.Write("]");

            Console.WriteLine();

            Console.Write("prices = [");
            for (int i = 0; i < prices.Length; i++)
            {
                Console.Write(prices[i] + " ");
            }
            Console.Write("]");

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Output = " + output);
            Console.WriteLine("Expected = " + expected);

            Console.WriteLine();
            Console.WriteLine(output == expected ? "Passed !" : "Wrong Answer !");
            Console.WriteLine();


            Console.WriteLine("===================");
            Console.WriteLine();

        }

        #endregion

    }
}
