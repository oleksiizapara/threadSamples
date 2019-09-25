using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadSamples.SemaphoreBook
{
    public class _3_8_Queue_Dancers
    {
        public void Invoke()
        {
            var n = 10;

            var random = new Random();
            var lockObj = new object();

            var semaphoreLeader = new SemaphoreSlim(0);
            var semaphoreSlave = new SemaphoreSlim(0);

            var dancerType = new bool[] { false, false, false, true, true, false, false, true, true, true };

            Func<bool, string> leader = (bool isLeader) => isLeader ? "Leader" : "Slave";

            var tasks = Enumerable.Range(0, n).ToList()
                .Select((x, index) => Task.Run(() =>
                {
                    bool isLeader = dancerType[index];

                    Console.WriteLine($"{leader(isLeader)} {index} started.");

                    if (isLeader)
                    {
                        semaphoreLeader.Release();
                        semaphoreSlave.Wait();
                    }
                    else
                    {
                        semaphoreSlave.Release();
                        semaphoreLeader.Wait();
                    }

                    Console.WriteLine($" -- {leader(isLeader)} {index} working.");


                    //Console.WriteLine($" --{leader(isLeader)} {index} finished.");
                })).ToArray();

            Task.WaitAll(tasks);
        }
    }
}
