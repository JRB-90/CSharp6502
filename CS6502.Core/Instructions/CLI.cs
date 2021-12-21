using System;

namespace CS6502.Core
{
    internal class CLI : InstructionBase
    {
        public CLI()
          :
            base(
                "CLI",
                0x58,
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
                    MicroCodeInstruction.ClearIRQ
                );
        }
    }
}
