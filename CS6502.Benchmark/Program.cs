using CS6502.Core;
using System;

namespace CS6502.Benchmark
{
    class Program
    {
        //const string FILE_NAME = "statusTests";
        //const string FILE_NAME = "transferTests";
        //const string FILE_NAME = "stackTests";
        //const string FILE_NAME = "subTests";
        //const string FILE_NAME = "compareTests";
        //const string FILE_NAME = "branchTests";
        //const string FILE_NAME = "loadStoreTests";
        const string FILE_NAME = "mathTests";
        const string CPU_PROG = "C:\\Development\\Sim6502\\asm\\asmtest\\build\\" + FILE_NAME + ".bin";
        const string BENCH_PATH = "C:\\Development\\Sim6502\\asm\\asmtest\\build\\" + FILE_NAME + ".csv";

        static void Main(string[] args)
        {
            Program p = new Program();
            p.LoadBenchmarkFromFile();
            //p.LoadBenchmarkFromP6502(100);
            p.RunBenchark(0);
            System.Console.ReadLine();
        }

        public Program()
        {
            benchmark = new BenchmarkSession();
        }

        public void LoadBenchmarkFromP6502(int cyclesToRun)
        {
            try
            {
                Console.WriteLine("Building benchmark from P6502 simulation...");
                benchmark.LoadFile(BENCH_PATH);
                Console.WriteLine("P6502 simulation ran successuflly, beginning test..");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load benchmarking file: {ex.Message}");
            }
        }

        public void LoadBenchmarkFromFile()
        {
            try
            {
                Console.WriteLine("Loading benchmark file...");
                benchmark.LoadFile(BENCH_PATH);
                Console.WriteLine("File loaded successuflly, beginning test..");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load benchmarking file: {ex.Message}");
            }
        }

        public void RunBenchark(int startingOffset)
        {
            try
            {
                benchmark.Run(CPU_PROG, startingOffset);
                Console.WriteLine("Test complete");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed run session: {ex.Message}");
            }
        }

        private BenchmarkSession benchmark;
    }
}
