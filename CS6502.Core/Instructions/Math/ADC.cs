using System;

namespace CS6502.Core
{
    internal class ADC : MathInstructionBase
    {
        public static ADC CreateADC(AddressingMode addressingMode)
        {
            switch (addressingMode)
            {
                case AddressingMode.Immediate:
                    return new ADC(0x69, addressingMode);

                case AddressingMode.ZeroPage:
                    return new ADC(0x65, addressingMode);

                case AddressingMode.ZeroPageX:
                    return new ADC(0x75, addressingMode);

                case AddressingMode.Absolute:
                    return new ADC(0x6D, addressingMode);

                case AddressingMode.AbsoluteX:
                    return new ADC(0x7D, addressingMode);

                case AddressingMode.AbsoluteY:
                    return new ADC(0x79, addressingMode);

                case AddressingMode.XIndirect:
                    return new ADC(0x61, addressingMode);

                case AddressingMode.IndirectY:
                    return new ADC(0x71, addressingMode);

                default:
                    throw new ArgumentException($"ADC does not support {addressingMode.ToString()} addressing mode");
            }
        }

        private ADC(
            byte opcode,
            AddressingMode addressingMode)
          :
            base(
                "ADC",
                opcode,
                addressingMode,
                MicroCodeInstruction.ADC)
        {
        }
    }
}
