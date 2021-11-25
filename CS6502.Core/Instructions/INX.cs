namespace CS6502.Core
{
    public class INX : InstructionBase
    {
        public INX()
          :
            base(
                "INX",
                0xE8,
                AddressingMode.Implied)
        {
        }

        public override void Execute(CpuRegisters registers)
        {
            registers.IncrementX();
        }
    }
}
