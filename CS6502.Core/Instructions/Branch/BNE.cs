using System;

namespace CS6502.Core
{
    internal class BNE : BranchInstructionBase
    {
        public BNE()
          :
            base(
                "BNE",
                0xD0,
                AddressingMode.Relative,
                p => p.ZeroFlag == false)
        {
        }
    }
}
