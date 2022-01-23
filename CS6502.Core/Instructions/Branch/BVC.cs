using System;

namespace CS6502.Core
{
    internal class BVC : BranchInstructionBase
    {
        public BVC()
          :
            base(
                "BVC",
                0x50,
                AddressingMode.Relative,
                p => p.OverflowFlag == false)
        {
        }
    }
}
