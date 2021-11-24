using CS6502.Core;
using System;

namespace CS6502.Benchmark
{
    class Program
    {
        const string BENCH_PATH = "C:\\Development\\Sim6502\\tests\\cpu.csv";
        const string CPU_PROG = "C:\\Development\\Sim6502\\tests\\main.bin";

        static void Main(string[] args)
        {
            Program p = new Program();
            p.LoadBenchmarkFromFile();
            //p.LoadBenchmarkFromP6502(100);
            p.RunBenchark();
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
                Environment.Exit(1);
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
                Environment.Exit(1);
            }
        }

        public void RunBenchark()
        {
            try
            {
                benchmark.Run(CPU_PROG);
                Console.WriteLine("Test complete");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed run session: {ex.Message}");
                Environment.Exit(1);
            }
        }

        private BenchmarkSession benchmark;
    }
}
