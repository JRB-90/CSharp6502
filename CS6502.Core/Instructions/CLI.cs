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
