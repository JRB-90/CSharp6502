namespace CS6502.Core
{
    /// <summary>
    /// Enum to enumerate the different addressing modes an instruction might have.
    /// </summary>
    public enum AddressingMode
    {
        Imediate,
        ZeroPage,
        ZeroPageX,
        ZeroPageY,
        Relative,
        Indirect,
        Implied,
        Absolute,
        AbsoluteX,
        AbsoluteY
    }
}
