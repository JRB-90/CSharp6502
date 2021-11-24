namespace CS6502.Core
{
    public class CLV : InstructionBase
    {
        public CLV()
          :
            base(
                "CLV",
                0xB8,
                AddressingMode.Implied)
        {
        }

        public override void Execute(CpuRegisters registers)
        {
            registers.ClearOverflow();
        }
    }
}
