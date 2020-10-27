using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Multithreading
{
    class Program
    {
        /*
         * Vragen:
         * 1. Wat is multithreading? Het tegelijkertijd(asynchroon) uitvoeren van verschillende taken door ze te verdelen over verschillende threads
         * 2. Wanneer gebruik je meerdere threads? Het is bijvoorbeeld handig om multithreading te gebruiken bij grote operaties. Deze kunnen van
         *      bijvoorbeeld op de achtergrond uitgevoerd worden zonder dat de mainthread verstoord wordt.
         * 3.  - Het delen van data tussen verschillende threads kan voor problemen zorgen.
         *     - Het afhandelen van foutmeldingen
         *     - Het gebruik van locks kan zorgen voor de verslechtering van de performance
         * 4.  - Objecten worden in de heap geplaatst. 
         *     - Bij multithreading wil je voorkomen dat verschillende threads gebruik maken van dezelfde resources.
         * 5.  - 
         * 6. Een racing condition is wanneer verschillende threads eenzelfde resource gebruiken en/of aanpassen. Het is dan moeilijk te bepalen
         *      of dat in de juiste volgorde gebeurt. Om dit te voorkomen zou je een lock kunnen gebruiken. Ook zou je ervoor kunnen zorgen dat de 
         *      verschillende threads niet hetzelfde object gebruiken.
        */

        const int AMOUNT = 100;
        const int THREADS = 5;
        static int amount = AMOUNT;
        static int amountOfSleep = 50;
        static object syncObj = new object();

        static void Main(string[] args)
        {
            SingleThreaded();
            MultiThreadedRaceCondition();
            MultiThreaded();
            Console.ReadLine();
        }

        static void ResetAmount()
        {
            amount = AMOUNT;
        }

        static void SingleThreaded()
        {
            ResetAmount();

            Stopwatch watch = new Stopwatch();
            watch.Start();

            while (amount > 0)
            {
                Thread.Sleep(amountOfSleep);
                amount--;
            }

            watch.Stop();
            TimeSpan timeSpan = watch.Elapsed;

            Console.WriteLine("Amount: {4}. Took single threaded {0}h {1}m {2}s {3}ms", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds, amount);
        }

        static void MultiThreadedRaceCondition()
        {
            ResetAmount();

            Stopwatch watch = new Stopwatch();
            watch.Start();

            //int splitAmount = AMOUNT / THREADS;
            Task[] taskArray = new Task[THREADS];
            for (int i = 0; i < taskArray.Length; i++)
            {
                taskArray[i] = Task.Factory.StartNew(() =>
                {
                    while (amount > 0)
                    {
                        Thread.Sleep(amountOfSleep);
                        amount--;
                    }
                });
            }
            Task.WaitAll(taskArray);

            watch.Stop();
            TimeSpan timeSpan = watch.Elapsed;

            Console.WriteLine("Amount: {4}. Took multi threaded {0}h {1}m {2}s {3}ms", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds, amount);
        }

        static void MultiThreaded()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            int splitAmount = AMOUNT / THREADS;
            int amount = AMOUNT;
            Task[] taskArray = new Task[THREADS];
            for (int i = 0; i < taskArray.Length; i++)
            {
                taskArray[i] = Task.Factory.StartNew(() =>
                {
                    int done = 0;
                    while (done < splitAmount)
                    {
                        Thread.Sleep(amountOfSleep);
                        done++;
                    }
                    amount -= done;
                });
            }
            Task.WaitAll(taskArray);

            watch.Stop();
            TimeSpan timeSpan = watch.Elapsed;

            Console.WriteLine("Amount: {4}. Took multi threaded {0}h {1}m {2}s {3}ms", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds, amount);
        }
    }
}
