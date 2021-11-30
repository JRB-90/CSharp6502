using System;

namespace CS6502.Core
{
    internal class INX : InstructionBase
    {
        public INX()
          :
            base(
                "INX",
                0xE8,
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
