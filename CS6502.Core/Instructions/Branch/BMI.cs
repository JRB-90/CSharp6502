using System;

namespace CS6502.Core
{
    internal class BMI : BranchInstructionBase
    {
        public BMI()
          :
            base(
                "BMI",
                0x30,
                AddressingMode.Relative,
                p => p.NegativeFlag == true)
        {
        }
    }
}
