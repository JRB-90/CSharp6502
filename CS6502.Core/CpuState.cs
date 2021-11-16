namespace CS6502.Core
{
    /// <summary>
    /// Enum to store the different internal states of the CPU.
    /// </summary>
    public enum CpuState
    {
        Init,
        ResetActive,
        IrqActive,
        NmiActive,
        BrkActive,
        Startup,
        ReadingOpcode,
        HandlingAddressingMode,
        ExecutingInstruction
    }
}
