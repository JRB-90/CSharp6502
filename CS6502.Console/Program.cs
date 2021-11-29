using CS6502.Core;
using System;

namespace CS6502.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            //BasicCpuSystem system =
            //    new BasicCpuSystem("C:\\Development\\Sim6502\\asm\\asmtest\\build\\main.bin");

            AccurateCpuSystem system =
                new AccurateCpuSystem("C:\\Development\\Sim6502\\asm\\asmtest\\build\\main.bin");

            for (int i = 0; i < 100; i++)
            {
                system.Cycle(true);
            }
        }
    }
}
