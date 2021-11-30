﻿using System;

namespace CS6502.Core
{
    internal class SEC : InstructionBase
    {
        public SEC()
          :
            base(
                "SEC",
                0x38,
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
