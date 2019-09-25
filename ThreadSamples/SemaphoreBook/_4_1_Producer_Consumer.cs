using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadSamples.SemaphoreBook
{
    public class _4_1_Producer_Consumer
    {

        public void Invoke()
        {
            //var lockObj = new object();
            var mutex = new SemaphoreSlim(1, 1);
            var bufSemaphore = new SemaphoreSlim(0, 1);
            var random = new Random();

            var buffer = new Queue<int>();

            var sw = Stopwatch.StartNew();

            var consumers = Enumerable.Range(0, 10).Select(i => Task.Run(async () =>
            {
                Console.WriteLine($"Consumer wait {i} {sw.ElapsedMilliseconds}");

                await bufSemaphore.WaitAsync();
                await mutex.WaitAsync();
                {
                    var item = buffer.Dequeue();
                    Console.WriteLine($"Consumer removed {i}  {sw.ElapsedMilliseconds}");
                }
                mutex.Release();

            })).ToArray();

            Console.WriteLine($"Producer Started  {sw.ElapsedMilliseconds}");

            var producers = Enumerable.Range(0, 10).Select(i => Task.Run(async () =>
           {
               var wait = random.Next(1000);
               await Task.Delay(wait);
               Console.WriteLine($"Producer invoked {i} wait {wait}  {sw.ElapsedMilliseconds}");

               await mutex.WaitAsync();
               {
                   buffer.Enqueue(i);
                   Console.WriteLine($"Producer added {i}  {sw.ElapsedMilliseconds}");
                   bufSemaphore.Release();
               }
               mutex.Release();

           })).ToArray();

            var tasks = producers.Union(consumers).ToList();

            //tasks.ForEach(x => x.Start(TaskScheduler.Default));

            Task.WaitAll(tasks.ToArray());


            Console.WriteLine($"Finished  {sw.ElapsedMilliseconds}");
        }

        public void InvokeThreads()
        {
            var lockObj = new object();
            var mutex = new SemaphoreSlim(1);
            var bufSemaphore = new SemaphoreSlim(0);
            var random = new Random();

            var buffer = new Queue<int>();

            var sw = Stopwatch.StartNew();

            var consumers = Enumerable.Range(0, 10).Select(i => new Thread(() =>
            {
                Console.WriteLine($"Consumer wait {i} {sw.ElapsedMilliseconds}");

                bufSemaphore.Wait();
                //mutex.Wait();

                lock (lockObj)
                {
                    var item = buffer.Dequeue();
                    Console.WriteLine($"Consumer removed {i}  {sw.ElapsedMilliseconds}");
                }
                //mutex.Release();

            })).ToArray();

            Console.WriteLine($"Producer Started  {sw.ElapsedMilliseconds}");

            var producers = Enumerable.Range(0, 10).Select(i => new Thread(() =>
            {
                var wait = random.Next(1000);
                Task.Delay(wait).Wait();
                Console.WriteLine($"Producer invoked {i} wait {wait}  {sw.ElapsedMilliseconds}");

                //mutex.Wait();
                lock (lockObj)
                {
                    buffer.Enqueue(i);
                    Console.WriteLine($"Producer added {i}  {sw.ElapsedMilliseconds}");
                    bufSemaphore.Release();
                }
                //mutex.Release();

            })).ToArray();

            var allThread = consumers.Union(producers).ToList();

            allThread.ForEach(x => x.Start());
            allThread.ForEach(x => x.Join());


            Console.WriteLine($"Finished  {sw.ElapsedMilliseconds}");
        }
    }
}
