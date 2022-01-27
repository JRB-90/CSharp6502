using System;

namespace CS6502.Core
{
    internal class CLV : InstructionBase
    {
        public CLV()
          :
            base(
                "CLV",
                0xB8,
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
                    MicroCodeInstruction.ClearOverflow
                );
        }
    }
}
