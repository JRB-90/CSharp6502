using System;

namespace CS6502.Core
{
    internal class DEX : InstructionBase
    {
        public DEX()
          :
            base(
                "DEX",
                0xCA,
                AddressingMode.Implied)
        {
        }

        public override CpuMicroCode Execute(SignalEdge signalEdge, int instructionCycle)
        {
            IsInstructionComplete = true;

            return
                new CpuMicroCode(
                    MicroCodeInstruction.DecrementX
                );
        }
    }
}
