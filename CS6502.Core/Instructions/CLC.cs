namespace CS6502.Core
{
    public class CLC : InstructionBase
    {
        public CLC()
          :
            base(
                "CLC",
                0x18,
                AddressingMode.Implied)
        {
        }

        public override void Execute(CpuRegisters registers)
        {
            registers.ClearCarry();
        }
    }
}
