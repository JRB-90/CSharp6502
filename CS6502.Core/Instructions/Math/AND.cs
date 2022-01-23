using System;

namespace CS6502.Core
{
    internal class AND : MathInstructionBase
    {
        public static AND CreateAND(AddressingMode addressingMode)
        {
            switch (addressingMode)
            {
                case AddressingMode.Immediate:
                    return new AND(0x29, addressingMode);

                case AddressingMode.ZeroPage:
                    return new AND(0x25, addressingMode);

                case AddressingMode.ZeroPageX:
                    return new AND(0x35, addressingMode);

                case AddressingMode.Absolute:
                    return new AND(0x2D, addressingMode);

                case AddressingMode.AbsoluteX:
                    return new AND(0x3D, addressingMode);

                case AddressingMode.AbsoluteY:
                    return new AND(0x39, addressingMode);

                case AddressingMode.XIndirect:
                    return new AND(0x21, addressingMode);

                case AddressingMode.IndirectY:
                    return new AND(0x31, addressingMode);

                default:
                    throw new ArgumentException($"AND does not support {addressingMode.ToString()} addressing mode");
            }
        }

        private AND(
            byte opcode,
            AddressingMode addressingMode)
          :
            base(
                "AND",
                opcode,
                addressingMode,
                MicroCodeInstruction.AND)
        {
        }
    }
}
