using System;

namespace CS6502.Core
{
    internal class BEQ : BranchInstructionBase
    {
        public BEQ()
          :
            base(
                "BEQ",
                0xF0,
                AddressingMode.Relative,
                p => p.ZeroFlag == true)
        {
        }
    }
}
