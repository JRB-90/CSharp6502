using System;

namespace CS6502.Benchmark
{
    class Program
    {
        const string BENCH_PATH = "C:\\Development\\Sim6502\\tests\\perfect6502cycles.csv";
        const string CPU_PROG = "C:\\Development\\Sim6502\\tests\\main.bin";

        static void Main(string[] args)
        {
            Console.WriteLine("Loading benchmark file...");

            BenchmarkSession benchmark = new BenchmarkSession();

            try
            {
                benchmark.LoadFIle(BENCH_PATH);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load benchmarking file: {ex.Message}");
                Environment.Exit(1);
            }

            try
            {
                Console.WriteLine("File loaded successuflly, beginning test..");
                benchmark.Run(CPU_PROG);
                Console.WriteLine("Test complete");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed run session: {ex.Message}");
                Environment.Exit(1);
            }
        }
    }
}
