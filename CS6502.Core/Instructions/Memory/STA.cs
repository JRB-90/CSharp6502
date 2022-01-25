using System;

namespace CS6502.Core
{
    internal class STA : MemoryInstructionBase
    {
        public static STA CreateSTA(AddressingMode addressingMode)
        {
            switch (addressingMode)
            {
                case AddressingMode.ZeroPage:
                    return new STA(0x85, addressingMode);

                case AddressingMode.ZeroPageX:
                    return new STA(0x95, addressingMode);

                case AddressingMode.Absolute:
                    return new STA(0x8D, addressingMode);

                case AddressingMode.AbsoluteX:
                    return new STA(0x9D, addressingMode);

                case AddressingMode.AbsoluteY:
                    return new STA(0x99, addressingMode);

                case AddressingMode.XIndirect:
                    return new STA(0x81, addressingMode);

                case AddressingMode.IndirectY:
                    return new STA(0x91, addressingMode);

                default:
                    throw new ArgumentException($"STA does not support {addressingMode.ToString()} addressing mode");
            }
        }

        private STA(
            byte opcode,
            AddressingMode addressingMode)
          :
            base(
                "STA",
                opcode,
                addressingMode,
                RWState.Write,
                MicroCodeInstruction.LatchAIntoDOR)
        {
        }
    }
}
