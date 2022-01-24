using System;

namespace CS6502.Core
{
    internal class LDY : LoadInstructionBase
    {
        public static LDY CreateLDY(AddressingMode addressingMode)
        {
            switch (addressingMode)
            {
                case AddressingMode.Immediate:
                    return new LDY(0xA0, addressingMode);

                case AddressingMode.ZeroPage:
                    return new LDY(0xA4, addressingMode);

                case AddressingMode.ZeroPageX:
                    return new LDY(0xB4, addressingMode);

                case AddressingMode.Absolute:
                    return new LDY(0xAC, addressingMode);

                case AddressingMode.AbsoluteX:
                    return new LDY(0xBC, addressingMode);

                default:
                    throw new ArgumentException($"LDY does not support {addressingMode.ToString()} addressing mode");
            }
        }

        private LDY(
            byte opcode,
            AddressingMode addressingMode)
          :
            base(
                "LDY",
                opcode,
                addressingMode,
                MicroCodeInstruction.LatchDILIntoY)
        {
        }
    }
}
