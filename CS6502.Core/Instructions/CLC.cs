namespace CS6502.Core
{
    public class CLC : InstructionBase
    {
        public CLC()
          :
            base(
                "CLC",
                0x18,
                AddressingMode.Implied,
                OperationType.Internal)
        {
        }

        public override void Execute(CpuRegisters registers)
        {
            registers.ClearCarry();
        }
    }
}
