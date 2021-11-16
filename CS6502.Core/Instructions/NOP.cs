namespace CS6502.Core
{
    /// <summary>
    /// NOP (No Operation) instruction, used as a blank instruction.
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
