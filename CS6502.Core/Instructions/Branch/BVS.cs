using System;

namespace CS6502.Core
{
    internal class BVS : BranchInstructionBase
    {
        public BVS()
          :
            base(
                "BVS",
                0x70,
                AddressingMode.Relative,
                p => p.OverflowFlag == true)
        {
        }
    }
}
