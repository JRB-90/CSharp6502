using System;

namespace CS6502.Core
{
    internal class LDA : MemoryInstructionBase
    {
        public static LDA CreateLDA(AddressingMode addressingMode)
        {
            switch (addressingMode)
            {
                case AddressingMode.Immediate:
                    return new LDA(0xA9, addressingMode);

                case AddressingMode.ZeroPage:
                    return new LDA(0xA5, addressingMode);

                case AddressingMode.ZeroPageX:
                    return new LDA(0xB5, addressingMode);

                case AddressingMode.Absolute:
                    return new LDA(0xAD, addressingMode);

                case AddressingMode.AbsoluteX:
                    return new LDA(0xBD, addressingMode);

                case AddressingMode.AbsoluteY:
                    return new LDA(0xB9, addressingMode);

                case AddressingMode.XIndirect:
                    return new LDA(0xA1, addressingMode);

                case AddressingMode.IndirectY:
                    return new LDA(0xB1, addressingMode);

                default:
                    throw new ArgumentException($"LDA does not support {addressingMode.ToString()} addressing mode");
            }
        }

        private LDA(
            byte opcode,
            AddressingMode addressingMode)
          :
            base(
                "LDA",
                opcode,
                addressingMode,
                RWState.Read,
                MicroCodeInstruction.LatchDILIntoA)
        {
        }
    }
}
