using System;
using System.Linq;
using System.Threading;

namespace ThreadSamples.SemaphoreBook
{
    public class _5_1_TheDiningSavagesProblem
    {
        Random rand = new Random();

        SemaphoreSlim potMutex = new SemaphoreSlim(1, 1);

        SemaphoreSlim fullPotSem = new SemaphoreSlim(0, 1);
        SemaphoreSlim emptyPotSem = new SemaphoreSlim(0, 1);

        int n = 5;

        int pot = 12;

        int m = 12;

        public void Invoke()
        {
            var savages = Enumerable.Range(0, n).Select(i => new Thread(() =>
            {
                while (true)
                {
                    potMutex.Wait();

                    if (pot == 0)
                    {
                        emptyPotSem.Release();
                        fullPotSem.Wait();
                        pot += m;
                    }

                    pot--;

                    GetServingFromPot(i);
                    potMutex.Release();
                    Eat(i);
                }
            })
            { Name = $"Savage {i}" })
                .ToList();

            var cook = new Thread(() =>
            {
                while (true)
                {
                    emptyPotSem.Wait();

                    PutSavageInPot();

                    fullPotSem.Release();
                }
            })
            { Name = "Cook" };


            savages.Add(cook);

            savages.ForEach(x => x.Start());
            savages.ForEach(x => x.Join());
        }

        private void PutSavageInPot()
        {
            Console.WriteLine($"Cook fill a pot");
        }

        private void Eat(int i)
        {
            Thread.Sleep(rand.Next(100));
            //Console.WriteLine($"Savage {i} - Eating");
        }

        private void GetServingFromPot(int i)
        {
            Console.WriteLine($"Savage {i} - Served");
        }
    }
}
