using System;

namespace CS6502.Core
{
    internal class INY : InstructionBase
    {
        public INY()
          :
            base(
                "INY",
                0xC8,
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
