using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadSamples.SemaphoreBook
{
    public class _3_8_2_Queue_Dancers
    {
        public void Invoke()
        {
            var n = 10;

            var lockObj = new object();

            var leaders = 0;
            var followers = 0;

            var mutex = new SemaphoreSlim(1);

            var semaphoreLeader = new SemaphoreSlim(0);
            var semaphoreFollower = new SemaphoreSlim(0);
            var rendezvous = new SemaphoreSlim(1);

            var dancerType = new bool[] { false, false, false, true, true, false, false, true, true, true };

            Func<bool, string> leader = (bool isLeader) => isLeader ? "Leader" : "Slave";

            var tasks = Enumerable.Range(0, n).ToList()
                .Select((x, index) => Task.Run(() =>
                {
                    bool isLeader = dancerType[index];

                    Console.WriteLine($"{leader(isLeader)} {index} started.");

                    if (isLeader)
                    {

                        mutex.Wait();
                        if (followers > 0)
                        {
                            followers--;
                            semaphoreFollower.Release();
                        }
                        else
                        {
                            followers++;
                            mutex.Release();
                            semaphoreLeader.Wait();
                        }

                        Console.WriteLine($" -- {leader(isLeader)} {index} working.");

                        rendezvous.Wait();
                        mutex.Release();
                    }
                    else
                    {
                        mutex.Wait();
                        if (leaders > 0)
                        {
                            leaders--;
                            semaphoreLeader.Release();
                        }
                        else
                        {
                            leaders++;
                            mutex.Release();
                            semaphoreFollower.Wait();
                        }

                        Console.WriteLine($" -- {leader(isLeader)} {index} working.");

                        rendezvous.Release();

                    }

                    Console.WriteLine($" -- {leader(isLeader)} {index} finished.");


                    //Console.WriteLine($" --{leader(isLeader)} {index} finished.");
                })).ToArray();

            Task.WaitAll(tasks);
        }
    }
}
