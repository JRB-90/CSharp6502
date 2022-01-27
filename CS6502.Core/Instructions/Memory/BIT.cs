using System;

namespace CS6502.Core
{
    internal class BIT : MemoryInstructionBase
    {
        public static BIT CreateBIT(AddressingMode addressingMode)
        {
            switch (addressingMode)
            {
                case AddressingMode.ZeroPage:
                    return new BIT(0x24, addressingMode);

                case AddressingMode.Absolute:
                    return new BIT(0x2C, addressingMode);

                default:
                    throw new ArgumentException($"BIT does not support {addressingMode.ToString()} addressing mode");
            }
        }

        private BIT(
            byte opcode,
            AddressingMode addressingMode)
          :
            base(
                "BIT",
                opcode,
                addressingMode,
                RWState.Read,
                MicroCodeInstruction.BIT)
        {
        }
    }
}
