using System;

namespace CS6502.Core
{
    internal class SED : InstructionBase
    {
        public SED()
          :
            base(
                "SED",
                0xF8,
                AddressingMode.Implied)
        {
        }

        public override CpuMicroCode Execute(SignalEdge signalEdge, int instructionCycle)
        {
            IsInstructionComplete = true;

            return
                new CpuMicroCode(
                    MicroCodeInstruction.SetDecimal
                );
        }
    }
}
