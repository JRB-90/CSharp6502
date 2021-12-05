namespace CS6502.Core
{
    /// <summary>
    /// Enum to enumerate the different addressing modes an instruction might have.
    /// </summary>
    internal enum AddressingMode
    {
        Immediate,
        ZeroPage,
        ZeroPageX,
        ZeroPageY,
        Relative,
        Indirect,
        XIndirect,
        IndirectY,
        Implied,
        Absolute,
        AbsoluteX,
        AbsoluteY
    }
}
