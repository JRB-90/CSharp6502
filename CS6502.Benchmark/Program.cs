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

        const string CURRENT_TEST_NAME  = "incrementTests";
        const string WORKING_DIR        = "C:\\Development\\Sim6502\\asm\\asmtest\\build\\";
        const string CPU_PROG           = WORKING_DIR + CURRENT_TEST_NAME + ".bin";
        const string BENCH_PATH         = WORKING_DIR + CURRENT_TEST_NAME + ".csv";

        static readonly string[] BENCH_FILES = 
        {
            STATUS_TESTS,
            TRANSFER_TESTS,
            STACK_TESTS,
            SUB_TESTS,
            COMP_TESTS,
            BRANCH_TESTS,
            LOAD_STORE_TESTS,
            MATH_TESTS,
            BIT_TESTS,
            INC_TESTS,
        };

        static void Main(string[] args)
        {
            Program p = new Program();
            //p.LoadBenchmarkFromFile();
            //p.LoadBenchmarkFromP6502(100);
            //p.RunBenchark(0);

            p.RunAllBenchmarks(0);

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

        public void RunAllBenchmarks(int startingOffset)
        {
            Console.WriteLine("Starting benchmarking session...\n");

            try
            {
                for (int i = 0; i < BENCH_FILES.Length; i++)
                {
                    Console.WriteLine($"Running {BENCH_FILES[i]} test...");
                    benchmark = new BenchmarkSession();
                    benchmark.LoadFile(WORKING_DIR + BENCH_FILES[i] + ".csv");
                    benchmark.Run(
                        WORKING_DIR + BENCH_FILES[i] + ".bin", 
                        startingOffset
                    );
                    Console.WriteLine("Test complete\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed run session: {ex.Message}");
            }

            Console.WriteLine("Benchmarking session complete");
        }

        private BenchmarkSession benchmark;
    }
}
