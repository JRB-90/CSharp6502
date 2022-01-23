using System;

namespace CS6502.Core
{
    internal class LSR : ShiftInstructionBase
    {
        public static LSR CreateLSR(AddressingMode addressingMode)
        {
            switch (addressingMode)
            {
                case AddressingMode.Immediate:
                    return new LSR(0x4A, addressingMode);

                case AddressingMode.ZeroPage:
                    return new LSR(0x46, addressingMode);

                case AddressingMode.ZeroPageX:
                    return new LSR(0x56, addressingMode);

                case AddressingMode.Absolute:
                    return new LSR(0x4E, addressingMode);

                case AddressingMode.AbsoluteX:
                    return new LSR(0x5E, addressingMode);

                default:
                    throw new ArgumentException($"LSR does not support {addressingMode.ToString()} addressing mode");
            }
        }

        private LSR(
            byte opcode,
            AddressingMode addressingMode)
          :
            base(
                "LSR",
                opcode,
                addressingMode,
                MicroCodeInstruction.LSR,
                MicroCodeInstruction.LSR_A)
        {
        }
    }
}
