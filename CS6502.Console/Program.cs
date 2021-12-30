using CS6502.Core;
using System;

namespace CS6502.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            BasicCpuSystem system =
                new BasicCpuSystem("C:\\Development\\Sim6502\\asm\\asmtest\\build\\loadStoreTests.bin");

            System.Console.WriteLine(CycleState.GetHeaderString('\t'));

            for (int i = 0; i < 200; i++)
            {
                system.Cycle(true);
            }

            System.Console.ReadLine();
        }
    }
}
