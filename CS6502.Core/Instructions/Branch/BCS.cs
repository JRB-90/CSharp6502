using System;

namespace CS6502.Core
{
    internal class BCS : BranchInstructionBase
    {
        public BCS()
          :
            base(
                "BCS",
                0xB0,
                AddressingMode.Relative,
                p => p.CarryFlag == true)
        {
        }
    }
}
