using System;

namespace CS6502.Core
{
    internal class SEC : InstructionBase
    {
        public SEC()
          :
            base(
                "SEC",
                0x38,
                AddressingMode.Implied)
        {
        }

        public override CpuMicroCode Execute(
            SignalEdge signalEdge,
            int instructionCycle,
            StatusRegister status)
        {
            IsInstructionComplete = true;

            return
                new CpuMicroCode(
                    MicroCodeInstruction.SetCarry
                );
        }
    }
}
