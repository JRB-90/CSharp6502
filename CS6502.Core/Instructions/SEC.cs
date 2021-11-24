﻿namespace CS6502.Core
{
    public class SEC : InstructionBase
    {
        public SEC()
          :
            base(
                "SEC",
                0x38,
                AddressingMode.Implied)
        {
        }

        public override void Execute(CpuRegisters registers)
        {
            registers.SetCarry();
        }
    }
}
