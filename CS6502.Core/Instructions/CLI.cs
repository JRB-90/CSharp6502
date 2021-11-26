namespace CS6502.Core
{
    public class CLI : InstructionBase
    {
        public CLI()
          :
            base(
                "CLI",
                0x58,
                AddressingMode.Implied,
                OperationType.Internal)
        {
        }

        public override void Execute(CpuRegisters registers)
        {
            registers.ClearIRQ();
        }
    }
}
