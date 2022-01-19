using CS6502.ASM;
using CS6502.Core;
using System;

namespace CS6502.Benchmark
{
    class Program
    {
        const string STATUS_TESTS       = "statusTests";
        const string TRANSFER_TESTS     = "transferTests";
        const string STACK_TESTS        = "stackTests";
        const string SUB_TESTS          = "subTests";
        const string COMP_TESTS         = "compareTests";
        const string BRANCH_TESTS       = "branchTests";
        const string LOAD_STORE_TESTS   = "loadStoreTests";
        const string MATH_TESTS         = "mathTests";
        const string BIT_TESTS          = "bitwiseTests";
        const string INC_TESTS          = "incrementTests";

        static readonly string[] BENCH_FILES = 
        {
            LOAD_STORE_TESTS,
            TRANSFER_TESTS,
            STATUS_TESTS,
            STACK_TESTS,
            SUB_TESTS,
            BIT_TESTS,
            MATH_TESTS,
            INC_TESTS,
            COMP_TESTS,
            BRANCH_TESTS,
        };

        static void Main(string[] args)
        {
            Program p = new Program();
            //p.RunEmbeddedBenchmark("statusTests");
            p.RunAllEmbeddedBenchmarks();
            System.Console.ReadLine();
        }

        public void RunAllEmbeddedBenchmarks()
        {
            Console.WriteLine("Starting benchmarking session...\n");

            try
            {
                for (int i = 0; i < BENCH_FILES.Length; i++)
                {
                    Console.WriteLine($"Running {BENCH_FILES[i]} test...");
                    RunEmbeddedBenchmark(BENCH_FILES[i]);
                    Console.WriteLine("Test complete\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed run session: {ex.Message}");
            }

            Console.WriteLine("Benchmarking session complete");
        }

        public void RunEmbeddedBenchmark(string name)
        {
            var testCSV = EmbeddedFileLoader.LoadBenchmarkCsvFile(name);
            var testBin = EmbeddedFileLoader.LoadCompiledBinFile(name);

            BenchmarkSession benchmark = new BenchmarkSession();
            benchmark.LoadFileFromString(testCSV);
            benchmark.Run(testBin);
        }
    }
}
