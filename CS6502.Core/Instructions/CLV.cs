using System;

namespace CS6502.Core
{
    internal class CLV : InstructionBase
    {
        public CLV()
          :
            base(
                "CLV",
                0xB8,
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
