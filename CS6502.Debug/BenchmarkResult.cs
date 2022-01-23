namespace CS6502.Debug
{
    public class BenchmarkResult
    {
        public BenchmarkResult(
            IReadOnlyCollection<ComparisonResult> successfulCycles,
            IReadOnlyCollection<ComparisonResult> failedCycles)
        {
            SuccessfulCycles = successfulCycles;
            FailedCycles = failedCycles;
        }

        public bool WasTestSuccessful =>
            FailedCycles.Count == 0;

        public IReadOnlyCollection<ComparisonResult> SuccessfulCycles { get; }

        public IReadOnlyCollection<ComparisonResult> FailedCycles { get; }

        public IReadOnlyCollection<int> FailedCycleIDs =>
            FailedCycles
            .Select(c => c.ActualState.CycleID)
            .ToList();
    }
}
