using CS6502.ASM;
using CS6502.Core;
using System;

namespace CS6502.Console
{
    class Program
    {
        const string PROG_NAME = "incrementTests";

        static void Main(string[] args)
        {
            BasicCpuSystem system =
                new BasicCpuSystem(
                    EmbeddedFileLoader.LoadCompiledBinFile(PROG_NAME)
                );

            System.Console.WriteLine(CycleState.GetHeaderString('\t'));

            for (int i = 0; i < 250; i++)
            {
                system.Cycle(true);
            }

            System.Console.ReadLine();
        }
    }
}
