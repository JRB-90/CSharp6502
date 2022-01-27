using System;

namespace CS6502.Core
{
    internal class ROL : ShiftInstructionBase
    {
        public static ROL CreateROL(AddressingMode addressingMode)
        {
            switch (addressingMode)
            {
                case AddressingMode.Immediate:
                    return new ROL(0x2A, addressingMode);

                case AddressingMode.ZeroPage:
                    return new ROL(0x26, addressingMode);

                case AddressingMode.ZeroPageX:
                    return new ROL(0x36, addressingMode);

                case AddressingMode.Absolute:
                    return new ROL(0x2E, addressingMode);

                case AddressingMode.AbsoluteX:
                    return new ROL(0x3E, addressingMode);

                default:
                    throw new ArgumentException($"ROL does not support {addressingMode.ToString()} addressing mode");
            }
        }

        private ROL(
            byte opcode,
            AddressingMode addressingMode)
          :
            base(
                "ROL",
                opcode,
                addressingMode,
                MicroCodeInstruction.ROL,
                MicroCodeInstruction.ROL_A)
        {
        }
    }
}
