using System;

namespace CS6502.Core
{
    /// <summary>
    /// Factory class for creating instructions from their opcode.
    /// </summary>
    public static class InstructionDecoder
    {
        public static IInstruction DecodeOpcode(byte opcode)
        {
            switch (opcode)
            {
                case 0X00:
                    return new BRK();
                case 0x18:
                    return new CLC();
                case 0x38:
                    return new SEC();
                case 0x4C:
                    return JMP.CreateJMP(AddressingMode.Absolute);
                case 0x58:
                    return new CLI();
                case 0x78:
                    return new SEI();
                case 0xB8:
                    return new CLV();
                case 0xD8:
                    return new CLD();
                case 0xEA:
                    return new NOP();
                case 0xF8:
                    return new SED();
                default:
                    throw new ArgumentException($"Opcode {opcode.ToHexString()} not supported yet");
            }
        }
    }
}
