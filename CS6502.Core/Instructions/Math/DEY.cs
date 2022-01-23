using System;

namespace CS6502.Core
{
    internal class DEY : InstructionBase
    {
        public DEY()
          :
            base(
                "DEY",
                0x88,
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
                    MicroCodeInstruction.DecrementY
                );
        }
    }
}
