namespace CS6502.Core
{
    /// <summary>
    /// Factory class for creating instructions from their opcode.
    /// </summary>
    public static class InstructionDecoder
    {
        public static IInstruction DecodeOpcode(byte opcode)
        {
            // TODO - switch on opcode
            return new NOP();
        }
    }
}
