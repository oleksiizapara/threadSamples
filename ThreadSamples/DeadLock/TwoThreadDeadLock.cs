using System;
using System.Threading.Tasks;

namespace ThreadSamples.DeadLock
{
    public class TwoThreadDeadLock
    {
        public void Invoke()
        {
            string resource1 = "Resource1";
            string resource2 = "Resource2";

            var task1 = Task.Run(() =>
            {
                Console.WriteLine($"attempt to lock {resource1}");
                lock (resource1)
                {
                    Console.WriteLine($"{resource1} locked");
                    Task.Delay(1000).Wait();

                    Console.WriteLine($"attempt to lock {resource2}");
                    lock (resource2)
                    {
                        Console.WriteLine($"{resource2} locked");
                    }
                }

                Console.WriteLine($"resources {resource1} {resource2} released");
            });

            var task2 = Task.Run(() =>
            {
                Console.WriteLine($"attempt to lock {resource2}");
                lock (resource2)
                {
                    Console.WriteLine($"{resource2} locked");
                    Task.Delay(1000).Wait();

                    Console.WriteLine($"attempt to lock {resource2}");
                    lock (resource1)
                    {
                        Console.WriteLine($"{resource1} locked");
                    }
                }

                Console.WriteLine($"resources {resource1} {resource2} released");
            });

            Task.WaitAll(task1, task2);
        }
    }
}
