namespace CS6502.Core
{
    internal enum DecodeState
    {
        ReadingOpcode,
        Addressing,
        Executing,
        Interupt
    }
}
