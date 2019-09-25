using System;
using System.Collections.Generic;
using System.Threading;

namespace ThreadSamples.SemaphoreBook
{
    public class _4_5_3_SmokerProblem
    {
        public void Invoke()
        {
            var rand = new Random();

            var mutex = new SemaphoreSlim(1, 1);

            var agentSem = new SemaphoreSlim(1, 1);

            var tobaco = new SemaphoreSlim(0);
            var paper = new SemaphoreSlim(0);
            var mutch = new SemaphoreSlim(0);

            var tobacoSem = new SemaphoreSlim(0);
            var paperSem = new SemaphoreSlim(0);
            var mutchSem = new SemaphoreSlim(0);

            var isTobaco = false;
            var isPaper = false;
            var isMutch = false;

            var threads = new List<Thread>
            {
                new Thread(() =>
                {
                    while(true) {
                        agentSem.Wait();
                        Console.WriteLine("Agent A tobaco and paper");
                        tobaco.Release();
                        paper.Release();
                        agentSem.Release();
                        Thread.Sleep(rand.Next(100));
                    }
                }) { Name = "Agent A" },
                new Thread(() =>
                {
                    while(true) {
                        agentSem.Wait();
                        Console.WriteLine("Agent B paper and mutch");
                        paper.Release();
                        mutch.Release();
                        agentSem.Release();
                        Thread.Sleep(rand.Next(100));
                    }
                }) { Name = "Agent B" },
                new Thread(() =>
                {
                    while(true) {
                        agentSem.Wait();
                        Console.WriteLine("Agent C tobaco and mutch");
                        tobaco.Release();
                        mutch.Release();
                        agentSem.Release();
                        Thread.Sleep(rand.Next(100));
                    }
                }) { Name = "Agent C" },

                new Thread(() =>
                {
                    while(true) {
                        mutchSem.Wait();
                        Console.WriteLine("Smoker with mutcher");
                    }
                }) { Name = "Smoker with mutcher" },
                new Thread(() =>
                {
                    while(true) {
                        paperSem.Wait();
                        Console.WriteLine("Smoker with paper");
                    }
                }) { Name = "Smoker with paper" },
                new Thread(() =>
                {
                    while(true) {
                        tobacoSem.Wait();
                        Console.WriteLine("Smoker with tobaco");
                    }
                }) { Name = "Smoker with tobaco" },

                new Thread(() =>
                {
                    while(true) {
                        tobaco.Wait();
                        mutex.Wait();

                        if(isPaper)
                        {
                            Console.WriteLine("Add-- Paper");
                            isPaper = false;
                            mutchSem.Release();
                        } else if (isMutch)
                        {
                            Console.WriteLine("Add-- Mutch");
                            isMutch = false;
                            paperSem.Release();
                        } else
                        {
                            isTobaco = true;
                            Console.WriteLine("Add Tobaco");
                        }

                        mutex.Release();
                    }

                }) { Name= "Pusher A tobaco" },

                new Thread(() =>
                {
                    while(true) {
                        paper.Wait();
                        mutex.Wait();

                        if(isTobaco)
                        {
                            Console.WriteLine("Add-- Tobaco");
                            isTobaco = false;
                            mutchSem.Release();
                        } else if (isMutch)
                        {
                            Console.WriteLine("Add-- Mutch");
                            isMutch = false;
                            tobacoSem.Release();
                        } else
                        {
                            isPaper = true;
                            Console.WriteLine("Add Paper");
                        }

                        mutex.Release();
                    }
                }) { Name= "Pusher B paper" },

                new Thread(() =>
                {
                    while(true) {
                        mutch.Wait();
                        mutex.Wait();

                        if(isPaper)
                        {
                            Console.WriteLine("Add-- Paper");
                            isPaper = false;
                            tobacoSem.Release();
                        } else if (isTobaco)
                        {
                            Console.WriteLine("Add-- Tobaco");
                            isTobaco = false;
                            paperSem.Release();
                        } else
                        {
                            isMutch = true;
                            Console.WriteLine("Add Mutch");
                        }

                        mutex.Release();
                    }
                }) { Name= "Pusher C mutch" },

            };

            threads.ForEach(x => x.Start());
            threads.ForEach(x => x.Join());

            Console.ReadKey();
        }
    }
}
