using System;

namespace CS6502.Core
{
    internal class LDX : LoadInstructionBase
    {
        public static LDX CreateLDX(AddressingMode addressingMode)
        {
            switch (addressingMode)
            {
                case AddressingMode.Immediate:
                    return new LDX(0xA2, addressingMode);

                case AddressingMode.ZeroPage:
                    return new LDX(0xA6, addressingMode);

                case AddressingMode.ZeroPageY:
                    return new LDX(0xB6, addressingMode);

                case AddressingMode.Absolute:
                    return new LDX(0xAE, addressingMode);

                case AddressingMode.AbsoluteY:
                    return new LDX(0xBE, addressingMode);

                default:
                    throw new ArgumentException($"LDX does not support {addressingMode.ToString()} addressing mode");
            }
        }

        private LDX(
            byte opcode,
            AddressingMode addressingMode)
          :
            base(
                "LDX",
                opcode,
                addressingMode,
                MicroCodeInstruction.LatchDILIntoX)
        {
        }
    }
}
