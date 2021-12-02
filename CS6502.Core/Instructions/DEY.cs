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
                AddressingMode.Implied,
                OperationType.Internal)
        {
        }

        public override CpuMicroCode Execute(SignalEdge signalEdge, int instructionCycle)
        {
            throw new NotImplementedException();
        }
    }
}
