using System;

namespace CS6502.Core
{
    internal class CLD : InstructionBase
    {
        public CLD()
          :
            base(
                "CLD",
                0xD8,
                AddressingMode.Implied)
        {
        }

        public override CpuMicroCode Execute(SignalEdge signalEdge, int instructionCycle)
        {
            IsInstructionComplete = true;

            return
                new CpuMicroCode(
                    MicroCodeInstruction.ClearDecimal
                );
        }
    }
}
