using System;
using System.Collections.Generic;
using System.Threading;

namespace ThreadSamples.SemaphoreBook
{
    public class _3_7_ReusableBarrier
    {
        public void Invoke()
        {
            var n = 10;
            var array = new List<Thread>();

            var lockObj = new object();
            var turnstile1 = new SemaphoreSlim(0);
            var turnstile2 = new SemaphoreSlim(1);

            var counter = 0;

            for (var i = 0; i < n; i++)
            {
                var number = i;

                var thread = new Thread(() =>
                {

                    Console.WriteLine($"Before Barrier {number}");

                    lock (lockObj)
                    {
                        counter++;

                        if (counter == n)
                        {
                            turnstile2.Wait();
                            turnstile1.Release();
                        }
                    }

                    turnstile1.Wait();
                    turnstile1.Release();

                    Console.WriteLine($"Critical Point {number}");

                    lock (lockObj)
                    {
                        counter--;

                        if (counter == 0)
                        {
                            turnstile1.Wait();
                            turnstile2.Release();
                        }
                    }

                    turnstile2.Wait();
                    turnstile2.Release();

                    Console.WriteLine($"After Barrier {number}");
                });

                array.Add(thread);
            }


            array.ForEach(x => x.Start());

            array.ForEach(x => x.Join());

            Console.WriteLine($"Finihsed");

            Console.ReadKey();

        }
    }
}
