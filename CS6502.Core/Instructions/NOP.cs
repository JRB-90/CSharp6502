namespace CS6502.Core
{
    /// <summary>
    /// NOP instruction, which does not operation, used as a blank instruction.
    /// </summary>
    public class NOP : InstructionBase
    {
        public NOP()
          :
            base(
                "NOP",
                0xEA,
                AddressingMode.Implied
            )
        {
        }
    }
}
