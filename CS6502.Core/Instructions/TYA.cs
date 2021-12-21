using System;

namespace CS6502.Core
{
    internal class TYA : InstructionBase
    {
        public TYA()
          :
            base(
                "TYA",
                0x98,
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
                    MicroCodeInstruction.TransferYToA
                );
        }
    }
}
