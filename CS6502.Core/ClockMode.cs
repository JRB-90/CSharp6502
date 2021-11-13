namespace CS6502.Core
{
    /// <summary>
    /// Enum to hold the different modes a clock generator can be in.
    /// </summary>
    public enum ClockMode
    {
        StepHalfCycle,
        StepFullCycle,
        StepInstruction,
        FreeRunning
    }
}
