using System;

namespace CS6502.Core
{
    internal class ORA : MathInstructionBase
    {
        public static ORA CreateORA(AddressingMode addressingMode)
        {
            switch (addressingMode)
            {
                case AddressingMode.Immediate:
                    return new ORA(0x09, addressingMode);

                case AddressingMode.ZeroPage:
                    return new ORA(0x05, addressingMode);

                case AddressingMode.ZeroPageX:
                    return new ORA(0x15, addressingMode);

                case AddressingMode.Absolute:
                    return new ORA(0x0D, addressingMode);

                case AddressingMode.AbsoluteX:
                    return new ORA(0x1D, addressingMode);

                case AddressingMode.AbsoluteY:
                    return new ORA(0x19, addressingMode);

                case AddressingMode.XIndirect:
                    return new ORA(0x01, addressingMode);

                case AddressingMode.IndirectY:
                    return new ORA(0x11, addressingMode);

                default:
                    throw new ArgumentException($"ORA does not support {addressingMode.ToString()} addressing mode");
            }
        }

        private ORA(
            byte opcode,
            AddressingMode addressingMode)
          :
            base(
                "ORA",
                opcode,
                addressingMode,
                MicroCodeInstruction.ORA)
        {
        }
    }
}
