using CS6502.Core;

namespace CS6502.Debug
{
    /// <summary>
    /// Holds the output of a comparisson between teo cycle states.
    /// </summary>
    public class ComparisonResult
    {
        public ComparisonResult(
            CycleState baselineState,
            CycleState actualState,
            CpuMatchState matchState)
        {
            BaselineState = baselineState;
            ActualState = actualState;
            MatchState = matchState;
        }

        public CycleState BaselineState { get; }

        public CycleState ActualState { get; }

        public CpuMatchState MatchState { get; }

        public bool IsCompleteMatch =>
            MatchState.IsCompleteMatch;

        public override string ToString()
        {
            return MatchState.ToString();
        }

        public static ComparisonResult CompareCycles(
            CycleState baselineState,
            CycleState actualState,
            int offset = 0)
        {
            if (baselineState.CycleID != actualState.CycleID + offset)
            {
                throw new ArgumentException(
                    $"Cannot compare cycles, ID's do not macth {baselineState.CycleID}<->{actualState.CycleID + offset}"
                );
            }

            CpuMatchState matchState =
                new CpuMatchState(
                    actualState.CycleID,
                    baselineState.RW == actualState.RW,
                    baselineState.A == actualState.A,
                    baselineState.X == actualState.X,
                    baselineState.Y == actualState.Y,
                    baselineState.IR == actualState.IR,
                    baselineState.P == actualState.P,
                    baselineState.SP == actualState.SP,
                    baselineState.PC == actualState.PC,
                    baselineState.Address == actualState.Address,
                    baselineState.Data == actualState.Data
                );

            return
                new ComparisonResult(
                        baselineState,
                        actualState,
                        matchState
                    );
        }
    }
}
