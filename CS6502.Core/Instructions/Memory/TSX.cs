using System;

namespace CS6502.Core
{
    internal class TSX : InstructionBase
    {
        public TSX()
          :
            base(
                "TSX",
                0xBA,
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
                    MicroCodeInstruction.TransferSPToX
                );
        }
    }
}
