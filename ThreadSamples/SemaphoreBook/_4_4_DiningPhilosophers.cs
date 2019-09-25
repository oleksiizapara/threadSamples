using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ThreadSamples.SemaphoreBook
{
    public class _4_4_DiningPhilosophers
    {
        private enum PhilosopherStatus
        {
            Thinking,
            Hangry,
            Eating
        }

        private readonly Random random = new Random();
        private readonly SemaphoreSlim mutex = new SemaphoreSlim(1, 1);
        private readonly int n;
        private readonly PhilosopherStatus[] status;
        private readonly SemaphoreSlim[] semaphores;
        private readonly List<Thread> threads;

        public _4_4_DiningPhilosophers(int n)
        {
            this.n = n;

            status = Enumerable.Range(0, n).Select(_ => PhilosopherStatus.Thinking).ToArray();

            semaphores = Enumerable.Range(0, n).Select(_ => new SemaphoreSlim(0, 1)).ToArray();

            threads = Enumerable.Range(0, n).Select(i => new Thread(() =>
            {
                while (true)
                {
                    Waiting(i);

                    GetFork(i);

                    Eating(i);

                    PutFork(i);
                }

            })).ToList();
        }

        private void PutFork(int i)
        {
            mutex.Wait();
            status[i] = PhilosopherStatus.Thinking;
            mutex.Release();

            Test(GetLeft(i));
            Test(GetRight(i));
        }

        private void GetFork(int i)
        {
            mutex.Wait();
            status[i] = PhilosopherStatus.Hangry;
            mutex.Release();

            Test(i);

            semaphores[i].Wait();
        }

        private int GetRight(int i)
        {
            return (i + 1) % n;
        }

        private int GetLeft(int i)
        {
            return (n + i - 1) % n;
        }

        public void Invoke()
        {
            threads.ForEach(x => x.Start());
            threads.ForEach(x => x.Join());
        }

        private void Test(int i)
        {
            if (status[i] == PhilosopherStatus.Hangry
                && status[GetLeft(i)] != PhilosopherStatus.Eating
                && status[GetRight(i)] != PhilosopherStatus.Eating)
            {
                status[i] = PhilosopherStatus.Eating;
                semaphores[i].Release();
            }
        }

        private void Waiting(int i)
        {
            var sleep = random.Next(10);
            //Console.WriteLine($"{nameof(Waiting)} {index} {sleep} started");
            Thread.Sleep(sleep);
            Console.WriteLine($"{nameof(Waiting)} -- {i} {status[i]} -- {sleep} finished \t\t {GetStatus()}");
        }

        private void Eating(int p)
        {
            var sleep = random.Next(100);
            Console.WriteLine($"{nameof(Eating)} -- {p} {status[p]}-- {sleep} started  \t\t {GetStatus()}");
            Thread.Sleep(sleep);
            //Console.WriteLine($"{nameof(Eating)} {index} {sleep} finished");
        }

        private string GetStatus()
        {
            var temp = new PhilosopherStatus[status.Length];
            mutex.Wait();
            try
            {
                status.CopyTo(temp, 0);
            }
            finally
            {
                mutex.Release();
            }


            return string.Join("\t", temp.Select(x => x.ToString().Substring(0, 1)));
        }
    }
}
