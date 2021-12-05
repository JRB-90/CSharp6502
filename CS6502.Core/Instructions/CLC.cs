using System;

namespace CS6502.Core
{
    internal class CLC : InstructionBase
    {
        public CLC()
          :
            base(
                "CLC",
                0x18,
                AddressingMode.Implied)
        {
        }

        public override CpuMicroCode Execute(SignalEdge signalEdge, int instructionCycle)
        {
            IsInstructionComplete = true;

            return
                new CpuMicroCode(
                    MicroCodeInstruction.ClearCarry
                );
        }
    }
}
