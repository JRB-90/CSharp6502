namespace CS6502.Core
{
    public class CLI : InstructionBase
    {
        public CLI()
          :
            base(
                "CLI",
                0x58,
                AddressingMode.Implied)
        {
        }

        public override void Execute(CpuRegisters registers)
        {
            registers.ClearIRQ();
        }
    }
}
