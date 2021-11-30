namespace CS6502.Core
{
    /// <summary>
    /// NOP (No Operation) instruction, used as a blank instruction.
    /// </summary>
    internal class NOP : InstructionBase
    {
        public NOP()
          :
            base(
                "NOP",
                0xEA,
                AddressingMode.Implied,
                OperationType.Internal
            )
        {
        }
    }
}
