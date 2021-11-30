using System;

namespace CS6502.Core
{
    internal class SED : InstructionBase
    {
        public SED()
          :
            base(
                "SED",
                0xF8,
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
