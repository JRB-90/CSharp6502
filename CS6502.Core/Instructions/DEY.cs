namespace CS6502.Core
{
    public class DEY : InstructionBase
    {
        public DEY()
          :
            base(
                "DEY",
                0x88,
                AddressingMode.Implied)
        {
        }

        public override void Execute(CpuRegisters registers)
        {
            registers.DecrementY();
        }
    }
}
