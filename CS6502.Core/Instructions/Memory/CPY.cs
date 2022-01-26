using System;

namespace CS6502.Core
{
    internal class CPY : MemoryInstructionBase
    {
        public static CPY CreateCPY(AddressingMode addressingMode)
        {
            switch (addressingMode)
            {
                case AddressingMode.Immediate:
                    return new CPY(0xC0, addressingMode);

                case AddressingMode.ZeroPage:
                    return new CPY(0xC4, addressingMode);

                case AddressingMode.Absolute:
                    return new CPY(0xCC, addressingMode);

                default:
                    throw new ArgumentException($"CPY does not support {addressingMode.ToString()} addressing mode");
            }
        }

        private CPY(
            byte opcode,
            AddressingMode addressingMode)
          :
            base(
                "CPY",
                opcode,
                addressingMode,
                RWState.Read,
                MicroCodeInstruction.CMP_Y)
        {
        }
    }
}
