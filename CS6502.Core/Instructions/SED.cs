namespace CS6502.Core
{
    public class SED : InstructionBase
    {
        public SED()
          :
            base(
                "SED",
                0xF8,
                AddressingMode.Implied,
                OperationType.Internal)
        {
        }

        public override void Execute(CpuRegisters registers)
        {
            registers.SetDecimal();
        }
    }
}
