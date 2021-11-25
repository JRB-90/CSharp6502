namespace CS6502.Core
{
    public class DEX : InstructionBase
    {
        public DEX()
          :
            base(
                "DEX",
                0xCA,
                AddressingMode.Implied)
        {
        }

        public override void Execute(CpuRegisters registers)
        {
            registers.DecrementX();
        }
    }
}
