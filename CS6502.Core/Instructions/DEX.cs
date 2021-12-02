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
