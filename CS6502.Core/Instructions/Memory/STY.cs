using System;

namespace CS6502.Core
{
    internal class STY : MemoryInstructionBase
    {
        public static STY CreateSTY(AddressingMode addressingMode)
        {
            switch (addressingMode)
            {
                case AddressingMode.ZeroPage:
                    return new STY(0x84, addressingMode);

                case AddressingMode.ZeroPageX:
                    return new STY(0x94, addressingMode);

                case AddressingMode.Absolute:
                    return new STY(0x8C, addressingMode);

                default:
                    throw new ArgumentException($"STX does not support {addressingMode.ToString()} addressing mode");
            }
        }

        private STY(
            byte opcode,
            AddressingMode addressingMode)
          :
            base(
                "STY",
                opcode,
                addressingMode,
                RWState.Write,
                MicroCodeInstruction.LatchYIntoDOR)
        {
        }
    }
}
