using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ThreadSamples.SemaphoreBook
{
    public class _3_6_Barrier
    {
        public void Invoke()
        {
            var n = 10;
            var array = new List<Thread>();

            var lockObj = new object();
            var semaphore = new SemaphoreSlim(0);

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

                        if (counter == n - 1)
                        {
                            semaphore.Release();
                        }
                    }

                    semaphore.Wait();
                    semaphore.Release();

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
