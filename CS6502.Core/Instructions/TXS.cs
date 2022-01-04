using System;

namespace CS6502.Core
{
    internal class TXS : InstructionBase
    {
        public TXS()
          :
            base(
                "TXS",
                0x9A,
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
                    MicroCodeInstruction.TransferXToSP
                );
        }
    }
}
