using System;
using System.Threading;

namespace ThreadSamples.DeadLock
{
    public class AddDifferentThreadSyncronizations
    {
        public void Invoke()
        {
            SimpleMutex();
            MonitorSympleSample();
            MonitorWIthLockToken();
            InterlockedSample();
            SemaphoreSlimSample();



        }

        private static void SemaphoreSlimSample()
        {
            var semaphore = new SemaphoreSlim(initialCount: 1, maxCount: 1);

            semaphore.Wait();
            try
            {

            }
            finally
            {
                semaphore.Release();

            }
        }

        private static void InterlockedSample()
        {
            int total = 1;
            long sum = 3;

            Interlocked.Add(ref total, 5);
            Interlocked.Read(ref sum);

            var read = Interlocked.CompareExchange(ref total, 0, 0);
        }

        private static void MonitorWIthLockToken()
        {
            var lockObject = new object();

            var lockToken = false;

            try
            {
                Monitor.TryEnter(lockObject, ref lockToken);
            }
            finally
            {
                if (lockToken)
                {
                    Monitor.Exit(lockObject);
                }
            }
        }

        private static void MonitorSympleSample()
        {
            var lockObject = new object();

            try
            {
                Monitor.Enter(lockObject);

            }
            finally
            {
                Monitor.Exit(lockObject);
            }
        }

        /// <summary>
        /// It is used for cross-process syncronization
        /// </summary>
        private static void SimpleMutex()
        {
            var mutex = new Mutex();

            try
            {
                mutex.WaitOne();

            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }
    }
}
