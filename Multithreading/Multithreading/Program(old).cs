using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Multithreading
{
    class Program
    {
        static List<int> primeNumbers = null;
        static int amountLeft = 0;
        static object syncObj = new object();
        static Stopwatch watch = null;

        public static IEnumerable<int> DistributeInteger(int total, int divider)
        {
            if (divider == 0)
            {
                yield return 0;
            }
            else
            {
                int rest = total % divider;
                double result = total / (double)divider;

                for (int i = 0; i < divider; i++)
                {
                    if (rest-- > 0)
                        yield return (int)Math.Ceiling(result);
                    else
                        yield return (int)Math.Floor(result);
                }
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Please insert amount of numbers to be checked...");
            int amount = int.Parse(Console.ReadLine());
            //set amount of numbers left for prime checking equals to inserted amount, for showing multithreading example
            amountLeft = amount;

            Console.WriteLine("Please insert amount of threads to be used...");
            int threads = int.Parse(Console.ReadLine());

            ExecuteSingleThreaded(amount);
            ExecuteMultipleThreadsSituation1(amount, threads);
            ExecuteMultipleThreadsSituation2(amount, threads);

            Console.ReadLine();
        }

        static void ExecuteSingleThreaded(int amount)
        {
            primeNumbers = new List<int>();
            watch = new Stopwatch();
            watch.Start();

            for (int i = 0; i < amount; i++)
            {
                if (isPrime(i))
                {
                    primeNumbers.Add(i);
                }
            }

            watch.Stop();
            TimeSpan timeSpan = watch.Elapsed;
            Console.WriteLine("Single threaded found {0} prime numbers in {1}h {2}m {3}s {4}ms", primeNumbers.Count, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
        }

        /// <summary>
        /// This method demonstrates multithreading while causing a race condition
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="threads"></param>
        static void ExecuteMultipleThreadsSituation1(int amount, int threads)
        {
            watch = new Stopwatch();
            primeNumbers = new List<int>();
            watch.Start();

            Task[] taskArray = new Task[threads];
            for (int i = 0; i < taskArray.Length; i++)
            {
                taskArray[i] = Task.Factory.StartNew(ChildThread);
            }
            Task.WaitAll(taskArray);

            watch.Stop();
            NotifyUserThreadsFinished();
        }

        /// <summary>
        /// This method demonstrates multithreading without any race condition
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="threads"></param>
        static void ExecuteMultipleThreadsSituation2(int amount, int threads)
        {
            watch = new Stopwatch();
            primeNumbers = new List<int>();
            Task[] taskArray = new Task[threads];
            int taskCounter = 0;
            int done = 0;
            watch.Start();
            foreach (int step in DistributeInteger(amount, threads))
            {
                taskArray[taskCounter] = Task.Factory.StartNew(() => ChildThread(done, step));
                done += step;
                taskCounter++;
            }
            Task.WaitAll(taskArray);

            watch.Stop();
            NotifyUserThreadsFinished();
        }

        static void NotifyUserThreadsFinished()
        {
            TimeSpan timeSpan = watch.Elapsed;
            Console.WriteLine("Multi threaded found {0} prime numbers in {1}h {2}m {3}s {4}ms", primeNumbers.Count, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
        }

        static void ChildThread(int from, int step)
        {
            int counter = 1;
            //while numbers are left for checking
            while (counter <= step)
            {
                int current = from + counter;
                //if numbers is a primenumber, add to list
                if (isPrime(current))
                {
                    primeNumbers.Add(current);
                }
                //always substract from the total amount of integers left for checkign
                counter++;
            }
        }

        static void ChildThread()
        {
            //while numbers are left for checking
            while (amountLeft >= 0)
            {
                //if numbers is a primenumber, add to list
                if (isPrime(amountLeft))
                {
                    primeNumbers.Add(amountLeft);
                }
                //always substract from the total amount of integers left for checkign
                amountLeft--;
            }
        }

        static bool isPrime(int number)
        {

            if (number == 1) return false;
            if (number == 2) return true;

            for (int i = 2; i <= Math.Ceiling(Math.Sqrt(number)); ++i)
            {
                if (number % i == 0) return false;
            }

            return true;
        }
    }
}
