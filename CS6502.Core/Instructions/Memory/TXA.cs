using System;

namespace CS6502.Core
{
    internal class TXA : InstructionBase
    {
        public TXA()
          :
            base(
                "TXA",
                0x8A,
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
                    MicroCodeInstruction.TransferXToA
                );
        }
    }
}
