using System;
using System.Collections.Generic;
using System.Threading;

namespace ThreadSamples.SemaphoreBook
{
    public class _3_7_ReusableBarrier_BarrierClass
    {
        public void Invoke()
        {
            var n = 10;
            var array = new List<Thread>();

            var barrier = new Barrier(n);

            for (var i = 0; i < n; i++)
            {
                var number = i;

                var thread = new Thread(() =>
                {
                    Console.WriteLine($"Before Barrier {number}");

                    barrier.SignalAndWait();

                    Console.WriteLine($"Critical Point {number}");

                    barrier.SignalAndWait();

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
