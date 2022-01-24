using System;

namespace CS6502.Core
{
    internal class STX : StoreInstructionBase
    {
        public static STX CreateSTX(AddressingMode addressingMode)
        {
            switch (addressingMode)
            {
                case AddressingMode.ZeroPage:
                    return new STX(0x86, addressingMode);

                case AddressingMode.ZeroPageY:
                    return new STX(0x96, addressingMode);

                case AddressingMode.Absolute:
                    return new STX(0x8E, addressingMode);

                default:
                    throw new ArgumentException($"STX does not support {addressingMode.ToString()} addressing mode");
            }
        }

        private STX(
            byte opcode,
            AddressingMode addressingMode)
          :
            base(
                "STX",
                opcode,
                addressingMode,
                MicroCodeInstruction.LatchXIntoDOR)
        {
        }
    }
}
