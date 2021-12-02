namespace CS6502.Core
{
    internal class BRK : InstructionBase
    {
        public BRK()
          :
            base(
                "BRK",
                0x00,
                AddressingMode.Implied,
                OperationType.Internal)
        {
        }
    }
}
