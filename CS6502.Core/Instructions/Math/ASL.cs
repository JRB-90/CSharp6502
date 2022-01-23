using System;

namespace CS6502.Core
{
    internal class ASL : ShiftInstructionBase
    {
        public static ASL CreateASL(AddressingMode addressingMode)
        {
            switch (addressingMode)
            {
                case AddressingMode.Immediate:
                    return new ASL(0x0A, addressingMode);

                case AddressingMode.ZeroPage:
                    return new ASL(0x06, addressingMode);

                case AddressingMode.ZeroPageX:
                    return new ASL(0x16, addressingMode);

                case AddressingMode.Absolute:
                    return new ASL(0x0E, addressingMode);

                case AddressingMode.AbsoluteX:
                    return new ASL(0x1E, addressingMode);

                default:
                    throw new ArgumentException($"ASL does not support {addressingMode.ToString()} addressing mode");
            }
        }

        private ASL(
            byte opcode,
            AddressingMode addressingMode)
          :
            base(
                "ASL",
                opcode,
                addressingMode,
                MicroCodeInstruction.ASL,
                MicroCodeInstruction.ASL_A)
        {
        }
    }
}
