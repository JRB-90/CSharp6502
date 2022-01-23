using System;

namespace CS6502.Core
{
    internal class BPL : BranchInstructionBase
    {
        public BPL()
          :
            base(
                "BPL",
                0x10,
                AddressingMode.Relative,
                p => p.NegativeFlag == false)
        {
        }
    }
}
