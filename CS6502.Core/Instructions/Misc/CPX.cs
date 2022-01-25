using System;

namespace CS6502.Core
{
    internal class CPX : MemoryInstructionBase
    {
        public static CPX CreateCPX(AddressingMode addressingMode)
        {
            switch (addressingMode)
            {
                case AddressingMode.Immediate:
                    return new CPX(0xE0, addressingMode);

                case AddressingMode.ZeroPage:
                    return new CPX(0xE4, addressingMode);

                case AddressingMode.Absolute:
                    return new CPX(0xEC, addressingMode);

                default:
                    throw new ArgumentException($"CPX does not support {addressingMode.ToString()} addressing mode");
            }
        }

        private CPX(
            byte opcode,
            AddressingMode addressingMode)
          :
            base(
                "CPX",
                opcode,
                addressingMode,
                RWState.Read,
                MicroCodeInstruction.CMP_X)
        {
        }
    }
}
