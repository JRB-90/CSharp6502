using System;

namespace CS6502.Core
{
    internal class CMP : LoadInstructionBase
    {
        public static CMP CreateCMP(AddressingMode addressingMode)
        {
            switch (addressingMode)
            {
                case AddressingMode.Immediate:
                    return new CMP(0xC9, addressingMode);

                case AddressingMode.ZeroPage:
                    return new CMP(0xC5, addressingMode);

                case AddressingMode.ZeroPageX:
                    return new CMP(0xD5, addressingMode);

                case AddressingMode.Absolute:
                    return new CMP(0xCD, addressingMode);

                case AddressingMode.AbsoluteX:
                    return new CMP(0xDD, addressingMode);

                case AddressingMode.AbsoluteY:
                    return new CMP(0xD9, addressingMode);

                case AddressingMode.XIndirect:
                    return new CMP(0xC1, addressingMode);

                case AddressingMode.IndirectY:
                    return new CMP(0xD1, addressingMode);

                default:
                    throw new ArgumentException($"CMP does not support {addressingMode.ToString()} addressing mode");
            }
        }

        private CMP(
            byte opcode,
            AddressingMode addressingMode)
          :
            base(
                "CMP",
                opcode,
                addressingMode,
                MicroCodeInstruction.CMP_A)
        {
        }
    }
}
