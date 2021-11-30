using System;

namespace CS6502.Core
{
    internal class CLC : InstructionBase
    {
        public CLC()
          :
            base(
                "CLC",
                0x18,
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
