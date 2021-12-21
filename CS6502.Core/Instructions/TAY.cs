using System;

namespace CS6502.Core
{
    internal class TAY : InstructionBase
    {
        public TAY()
          :
            base(
                "TAY",
                0xA8,
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
                    MicroCodeInstruction.TransferAToY
                );
        }
    }
}
