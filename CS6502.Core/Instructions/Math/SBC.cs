using System;

namespace CS6502.Core
{
    internal class SBC : MathInstructionBase
    {
        public static SBC CreateSBC(AddressingMode addressingMode)
        {
            switch (addressingMode)
            {
                case AddressingMode.Immediate:
                    return new SBC(0xE9, addressingMode);

                case AddressingMode.ZeroPage:
                    return new SBC(0xE5, addressingMode);

                case AddressingMode.ZeroPageX:
                    return new SBC(0xF5, addressingMode);

                case AddressingMode.Absolute:
                    return new SBC(0xED, addressingMode);

                case AddressingMode.AbsoluteX:
                    return new SBC(0xFD, addressingMode);

                case AddressingMode.AbsoluteY:
                    return new SBC(0xF9, addressingMode);

                case AddressingMode.XIndirect:
                    return new SBC(0xE1, addressingMode);

                case AddressingMode.IndirectY:
                    return new SBC(0xF1, addressingMode);

                default:
                    throw new ArgumentException($"SBC does not support {addressingMode.ToString()} addressing mode");
            }
        }

        private SBC(
            byte opcode,
            AddressingMode addressingMode)
          :
            base(
                "SBC",
                opcode,
                addressingMode,
                MicroCodeInstruction.SBC)
        {
        }
    }
}
