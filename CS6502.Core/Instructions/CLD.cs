using System;

namespace CS6502.Core
{
    internal class CLD : InstructionBase
    {
        public CLD()
          :
            base(
                "CLD",
                0xD8,
                AddressingMode.Implied,
                OperationType.Internal)
        {
        }

        public override CpuMicroCode Execute(int instructionCycle)
        {
            throw new NotImplementedException();
        }
    }
}
