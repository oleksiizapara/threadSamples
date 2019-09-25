using System;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadSamples.SemaphoreBook
{
    public class _3_3_Rednezovus
    {
        public void Invoke()
        {
            var semaphoreA = new SemaphoreSlim(0, 1);

            var semaphoreB = new SemaphoreSlim(0, 1);

            var thread1 = new Thread(() =>
            {
                Console.WriteLine("statemant A1");
                semaphoreB.Release();
                semaphoreA.Wait();
                Console.WriteLine("statemant A2");
            })
            { Name = "thread1" };

            var thread2 = new Thread(() =>
            {
                Console.WriteLine("statemant B1");
                semaphoreA.Release();
                semaphoreB.Wait();
                Console.WriteLine("statemant B2");

            })
            { Name = "thread2" };

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();

            Console.WriteLine($"Finished");

            Console.ReadKey();
        }
    }
}
