namespace CS6502.Core
{
    internal class BRK : InstructionBase
    {
        public BRK()
          :
            base(
                "BRK",
                0x00,
                AddressingMode.Implied)
        {
        }

        public override CpuMicroCode Execute(
            SignalEdge signalEdge, 
            int instructionCycle,
            StatusRegister status,
            bool wasPageBoundaryCrossed)
        {
            IsInstructionComplete = true;

            return
                new CpuMicroCode(
                    MicroCodeInstruction.BRK
                );
        }
    }
}
