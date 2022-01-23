using System;

namespace CS6502.Core
{
    internal class BCC : BranchInstructionBase
    {
        public BCC()
          :
            base(
                "BCC",
                0x90,
                AddressingMode.Relative,
                p => p.CarryFlag == false)
        {
        }
    }
}
