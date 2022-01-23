using CS6502.ASM;
using CS6502.Core;
using CS6502.Debug;
using System;
using System.Collections.Generic;
using System.Linq;

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

            p.RunSingleTest("statusTests");
            //p.RunAllEmbeddedBenchmarks();

            System.Console.ReadLine();
        }

        public void RunSingleTest(string name)
        {
            Console.WriteLine("Starting benchmarking session...\n");

            Console.WriteLine($"Running {name} test...");
            BenchmarkResult result = RunEmbeddedBenchmark(name);

            if (!result.WasTestSuccessful)
            {
                Console.WriteLine(CycleState.GetHeaderString('\t'));
            }

            foreach (var failedCycle in result.FailedCycles)
            {
                Console.WriteLine(failedCycle.ToString());
            }
        }

        public void RunAllEmbeddedBenchmarks()
        {
            List<BenchmarkResult> results = new List<BenchmarkResult>();
            Console.WriteLine("Starting benchmarking session...\n");

            try
            {
                for (int i = 0; i < BENCH_FILES.Length; i++)
                {
                    Console.WriteLine($"Running {BENCH_FILES[i]} test...");
                    results.Add(RunEmbeddedBenchmark(BENCH_FILES[i]));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed run session: {ex.Message}");
            }

            Console.WriteLine("Benchmarking session complete");

            if (results.Where(r => r.WasTestSuccessful == false).Count() > 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("=== Benchmark failed ===");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("=== Benchmark passed ===");
            }
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public BenchmarkResult RunEmbeddedBenchmark(string name)
        {
            var testCSV = EmbeddedFileLoader.LoadBenchmarkCsvFile(name);
            var testBin = EmbeddedFileLoader.LoadCompiledBinFile(name);

            BenchmarkSession benchmark = new BenchmarkSession();
            benchmark.LoadFileFromString(testCSV);
            BenchmarkResult result = benchmark.Run(testBin);

            if (result.WasTestSuccessful)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Test successful");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Test failed - {result.FailedCycles.Count} Cycles are mismatched");
            }
            Console.ForegroundColor = ConsoleColor.Gray;

            return result;
        }
    }
}
