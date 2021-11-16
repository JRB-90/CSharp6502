﻿namespace CS6502.Core
{
    public class BRK : InstructionBase
    {
        public BRK()
          :
            base(
                "BRK",
                0x00,
                AddressingMode.Implied)
        {
        }
    }
}