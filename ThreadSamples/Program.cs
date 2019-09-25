using System;
using System.Text.RegularExpressions;
using ThreadSamples.DeadLock;
using ThreadSamples.SemaphoreBook;

namespace ThreadSamples
{
    class Program
    {
        static void Main(string[] args)
        {
            //var twoThreadDeadLock = new TwoThreadDeadLock();

            //twoThreadDeadLock.Invoke();

            //new _3_3_Rednezovus().Invoke();

            //new _3_6_Barrier().Invoke();

            //new _3_7_ReusableBarrier().Invoke();

            //new _3_7_ReusableBarrier_BarrierClass().Invoke();

            //new _3_8_Queue_Dancers().Invoke();

            //new _3_8_2_Queue_Dancers().Invoke();

            //new _4_1_Producer_Consumer().Invoke();

            //new _4_2_ReaderWriter().Invoke();

            //new _4_4_DiningPhilosophers(5).Invoke();

            //new _4_5_3_SmokerProblem().Invoke();

            new _5_1_TheDiningSavagesProblem().Invoke();

        }
    }
}
