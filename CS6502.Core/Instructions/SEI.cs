﻿using System;

namespace CS6502.Core
{
    internal class SEI : InstructionBase
    {
        public SEI()
          :
            base(
                "SEI",
                0x78,
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
