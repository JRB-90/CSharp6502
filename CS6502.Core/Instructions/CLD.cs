namespace CS6502.Core
{
    public class CLD : InstructionBase
    {
        public CLD()
          :
            base(
                "CLD",
                0xD8,
                AddressingMode.Implied,
                OperationType.Internal)
        {
        }

        public override void Execute(CpuRegisters registers)
        {
            registers.ClearDecimal();
        }
    }
}
