using System;

namespace CS6502.Core
{
    internal class EOR : MathInstructionBase
    {
        public static EOR CreateEOR(AddressingMode addressingMode)
        {
            switch (addressingMode)
            {
                case AddressingMode.Immediate:
                    return new EOR(0x49, addressingMode);

                case AddressingMode.ZeroPage:
                    return new EOR(0x45, addressingMode);

                case AddressingMode.ZeroPageX:
                    return new EOR(0x55, addressingMode);

                case AddressingMode.Absolute:
                    return new EOR(0x4D, addressingMode);

                case AddressingMode.AbsoluteX:
                    return new EOR(0x5D, addressingMode);

                case AddressingMode.AbsoluteY:
                    return new EOR(0x59, addressingMode);

                case AddressingMode.XIndirect:
                    return new EOR(0x41, addressingMode);

                case AddressingMode.IndirectY:
                    return new EOR(0x51, addressingMode);

                default:
                    throw new ArgumentException($"EOR does not support {addressingMode.ToString()} addressing mode");
            }
        }

        private EOR(
            byte opcode,
            AddressingMode addressingMode)
          :
            base(
                "EOR",
                opcode,
                addressingMode,
                MicroCodeInstruction.EOR)
        {
        }
    }
}
