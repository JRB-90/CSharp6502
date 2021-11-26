namespace CS6502.Core
{
    public class DEX : InstructionBase
    {
        public DEX()
          :
            base(
                "DEX",
                0xCA,
                AddressingMode.Implied,
                OperationType.Internal)
        {
        }

        public override void Execute(CpuRegisters registers)
        {
            registers.DecrementX();
        }
    }
}
