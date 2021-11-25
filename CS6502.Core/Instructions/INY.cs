namespace CS6502.Core
{
    public class INY : InstructionBase
    {
        public INY()
          :
            base(
                "INY",
                0xC8,
                AddressingMode.Implied)
        {
        }

        public override void Execute(CpuRegisters registers)
        {
            registers.IncrementY();
        }
    }
}
