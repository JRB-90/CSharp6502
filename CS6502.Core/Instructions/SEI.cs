namespace CS6502.Core
{
    public class SEI : InstructionBase
    {
        public SEI()
          :
            base(
                "SEI",
                0x78,
                AddressingMode.Implied,
                OperationType.Internal)
        {
        }

        public override void Execute(CpuRegisters registers)
        {
            registers.SetIRQ();
        }
    }
}
