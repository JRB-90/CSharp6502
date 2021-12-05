using System;

namespace CS6502.Core
{
    internal class SEI : InstructionBase
    {
        public SEI()
          :
            base(
                "SEI",
                0x78,
                AddressingMode.Implied)
        {
        }

        public override CpuMicroCode Execute(SignalEdge signalEdge, int instructionCycle)
        {
            IsInstructionComplete = true;

            return
                new CpuMicroCode(
                    MicroCodeInstruction.SetIRQ
                );
        }
    }
}
