namespace CS6502.Core
{
    /// <summary>
    /// Enum to enumerate the different addressing modes an instruction might have.
    /// </summary>
    public enum AddressingMode
    {
        Immediate,
        ZeroPage,
        ZeroPageX,
        ZeroPageY,
        Relative,
        Indirect,
        IndirectX,
        IndirectY,
        Implied,
        Absolute,
        AbsoluteX,
        AbsoluteY
    }
}
