using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadSamples.SemaphoreBook
{
    public class _4_2_ReaderWriter
    {
        public void Invoke()
        {
            //var lockObj = new object();
            var readerWriterLock = new ReaderWriterLockSlim();

            var random = new Random();

            var value = 10;

            var sw = Stopwatch.StartNew();

            var readers = Enumerable.Range(0, 10).Select(i => new Thread(() =>
            {
                //    Console.WriteLine($"Reader {i} started {sw.ElapsedMilliseconds}");
                var sleep = random.Next(5000);
                Thread.Sleep(sleep);

                readerWriterLock.EnterReadLock();
                try
                {
                    Console.WriteLine($"Reader {i} locked value {value} {sw.ElapsedMilliseconds}");
                }
                finally
                {
                    readerWriterLock.ExitReadLock();
                }

                //Console.WriteLine($"Reader finished {i} {sw.ElapsedMilliseconds}");

            })).ToArray();

            Console.WriteLine($"Writer Started {sw.ElapsedMilliseconds}");

            var writers = Enumerable.Range(0, 2).Select(i => new Thread(() =>
            {
                var wait = random.Next(1000);
                Thread.Sleep(wait);
                //Console.WriteLine($"Writer {i} invoked wait {wait} {sw.ElapsedMilliseconds}");
                readerWriterLock.EnterWriteLock();
                try
                {
                    value = random.Next(50);

                    Console.WriteLine($"Writer {i} value {value} {sw.ElapsedMilliseconds}");
                }
                finally
                {
                    readerWriterLock.ExitWriteLock();
                }

            })).ToArray();

            var threads = readers.Union(writers).ToList();

            threads.ForEach(x => x.Start());
            threads.ForEach(x => x.Join());

            Console.WriteLine($"Finished  {sw.ElapsedMilliseconds}");

        }
    }
}
