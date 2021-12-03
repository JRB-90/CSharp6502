﻿using System;

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

        public override CpuMicroCode Execute(SignalEdge signalEdge, int instructionCycle)
        {
            IsInstructionComplete = true;

            return
                new CpuMicroCode(
                    MicroCodeInstruction.IncrementX
                );
        }
    }
}
