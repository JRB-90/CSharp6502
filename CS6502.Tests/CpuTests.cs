using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using CS6502.Debug;
using CS6502.ASM;

namespace CS6502.Tests
{
    [TestClass]
    public class CpuTests
    {
        const string STATUS_TESTS = "statusTests";
        const string TRANSFER_TESTS = "transferTests";
        const string STACK_TESTS = "stackTests";
        const string SUB_TESTS = "subTests";
        const string COMP_TESTS = "compareTests";
        const string BRANCH_TESTS = "branchTests";
        const string LOAD_STORE_TESTS = "loadStoreTests";
        const string MATH_TESTS = "mathTests";
        const string BIT_TESTS = "bitwiseTests";
        const string INC_TESTS = "incrementTests";

        [TestMethod]
        public void LoadStoreTests()
        {
            var result = RunEmbeddedBenchmark(LOAD_STORE_TESTS);

            result.WasTestSuccessful.Should().BeTrue();
            result.FailedCycles.Count.Should().Be(0);
            result.SuccessfulCycles.Count.Should().BePositive();
        }

        [TestMethod]
        public void TransferTests()
        {
            var result = RunEmbeddedBenchmark(TRANSFER_TESTS);

            result.WasTestSuccessful.Should().BeTrue();
            result.FailedCycles.Count.Should().Be(0);
            result.SuccessfulCycles.Count.Should().BePositive();
        }

        [TestMethod]
        public void StatusTests()
        {
            var result = RunEmbeddedBenchmark(STATUS_TESTS);

            result.WasTestSuccessful.Should().BeTrue();
            result.FailedCycles.Count.Should().Be(0);
            result.SuccessfulCycles.Count.Should().BePositive();
        }

        [TestMethod]
        public void StackTests()
        {
            var result = RunEmbeddedBenchmark(STACK_TESTS);

            result.WasTestSuccessful.Should().BeTrue();
            result.FailedCycles.Count.Should().Be(0);
            result.SuccessfulCycles.Count.Should().BePositive();
        }

        [TestMethod]
        public void SubroutineTests()
        {
            var result = RunEmbeddedBenchmark(SUB_TESTS);

            result.WasTestSuccessful.Should().BeTrue();
            result.FailedCycles.Count.Should().Be(0);
            result.SuccessfulCycles.Count.Should().BePositive();
        }

        [TestMethod]
        public void BitwiseTests()
        {
            var result = RunEmbeddedBenchmark(BIT_TESTS);

            result.WasTestSuccessful.Should().BeTrue();
            result.FailedCycles.Count.Should().Be(0);
            result.SuccessfulCycles.Count.Should().BePositive();
        }

        [TestMethod]
        public void MathTests()
        {
            var result = RunEmbeddedBenchmark(MATH_TESTS);

            result.WasTestSuccessful.Should().BeTrue();
            result.FailedCycles.Count.Should().Be(0);
            result.SuccessfulCycles.Count.Should().BePositive();
        }

        [TestMethod]
        public void IncrementTests()
        {
            var result = RunEmbeddedBenchmark(INC_TESTS);

            result.WasTestSuccessful.Should().BeTrue();
            result.FailedCycles.Count.Should().Be(0);
            result.SuccessfulCycles.Count.Should().BePositive();
        }

        [TestMethod]
        public void CompareTests()
        {
            var result = RunEmbeddedBenchmark(COMP_TESTS);

            result.WasTestSuccessful.Should().BeTrue();
            result.FailedCycles.Count.Should().Be(0);
            result.SuccessfulCycles.Count.Should().BePositive();
        }

        [TestMethod]
        public void BranchTests()
        {
            var result = RunEmbeddedBenchmark(BRANCH_TESTS);

            result.WasTestSuccessful.Should().BeTrue();
            result.FailedCycles.Count.Should().Be(0);
            result.SuccessfulCycles.Count.Should().BePositive();
        }

        private BenchmarkResult RunEmbeddedBenchmark(string name)
        {
            var testCSV = EmbeddedFileLoader.LoadBenchmarkCsvFile(name);
            var testBin = EmbeddedFileLoader.LoadCompiledBinFile(name);

            BenchmarkSession benchmark = new BenchmarkSession();
            benchmark.LoadFileFromString(testCSV);
            BenchmarkResult result = benchmark.Run(testBin);

            return result;
        }
    }
}
